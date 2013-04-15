using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using WarSpot.Cloud.Common;
using WarSpot.Cloud.Storage.Models;
using WarSpot.Common;
using WarSpot.Contracts.Service;

namespace WarSpot.Cloud.Storage
{
	public static class Warehouse
	{

		#region AZURE SECTION VAR
		private const string CONNECTIONSTRING = "DataConnectionString";
		private const string DBCONNECTIONSTRING = "DBConnectionString";

		private static bool storageInitialized = false;
		private static object gate = new Object();

		private static CloudBlobClient blobStorage;
		private static CloudBlobContainer container;

		private static CloudQueue queue;
		private static CloudQueueClient queueClient;

		#endregion BLOB SECTION END

		#region DATABASE SECTION VAR
		public static DBContext db;
		#endregion DATABASE SECTION END

		static Warehouse()
		{
			InitializeStorage();
		}

		#region BLOB METHODS
		private static void InitializeStorage()
		{

			if (storageInitialized)
			{
				return;
			}

			lock (gate)
			{
				if (storageInitialized)
				{
					return;
				}

				try  // инициализируем BLOB-хранилище
				{
					// read account configuration settings

					var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");



					// create blob container for images
					blobStorage = storageAccount.CreateCloudBlobClient();
					container = blobStorage.GetContainerReference("intellects");
					container.CreateIfNotExist();

					// configure container for public access
					var permissions = container.GetPermissions();
					permissions.PublicAccess = BlobContainerPublicAccessType.Container;
					container.SetPermissions(permissions);

					// create queue for matches
					queueClient = storageAccount.CreateCloudQueueClient();
					queue = queueClient.GetQueueReference("queue");
					queue.CreateIfNotExist();

					// раньше DbconnectionString для EntityConnection парсилась RoleEnviroment.GetConfiguration, но парсилась как-то.. 
					// плохо. временно заменяю на прямое указание!
					//db = new DBContext(new EntityConnection(@"metadata=res://*/DBModel.csdl|res://*/DBModel.ssdl|res://*/DBModel.msl;provider=System.Data.SqlClient;provider connection string='data source=localhost\SQLEXPRESS;initial catalog=WarSpotDB;integrated security=True;multipleactiveresultsets=True;App=EntityFramework'")); // инициализируем базу данных

					db = new DBContext(Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.GetConfigurationSettingValue("DBConnectionString"));

				}
				catch (WebException)
				{
					throw new WebException("Storage services initialization failure. "
							 + "Check your storage account configuration settings. If running locally, "
							 + "ensure that the Development Storage service is running.");
				}
				catch (TypeInitializationException)
				{
					throw new WebException("Storage services initialization failure. "
							 + "Check your storage account configuration settings. If running locally, "
							 + "ensure that the Development Storage service is running.");
				}

				storageInitialized = true;
			}
		}

		public static bool UploadIntellect(Guid Account_ID, string name, byte[] data, string description)
		{
			if (db.Intellect == null)
			{
				Trace.WriteLine("db.Intellect == null");
			}

			string uniqueBlobName = string.Format("intellects/{0}/{1}", Account_ID.ToString(), name);

			db.AddToIntellect(Intellect.CreateIntellect(Guid.NewGuid(), name, Account_ID, description));
			db.SaveChanges();

			CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
			blob.UploadByteArray(data);

			return true;

		}

		public static byte[] DownloadIntellect(Guid intellectID)
		{
			string name1 = (from b in db.Intellect
											where b.Intellect_ID == intellectID
											select b.Intellect_Name).First<string>();

			Guid name2 = (from a in db.Account
										where a.Account_ID == ((from b in db.Intellect
																						where b.Intellect_ID == intellectID
																						select b.AccountAccount_ID).FirstOrDefault<Guid>())
										select a.Account_ID).First<Guid>();


			string neededname = string.Format("intellects/{0}/{1}", name2.ToString(), name1);
			CloudBlockBlob blob = container.GetBlockBlobReference(neededname);



			return blob.DownloadByteArray();
		}

		public static MemoryStream GetReplayAsMemoryStream(Guid gameID)
		{
			var neededname = (from g in db.Game
												where g.Game_ID == gameID
												select g.Replay).FirstOrDefault<string>();
			if (neededname != null)
			{
				CloudBlockBlob blob = container.GetBlockBlobReference(neededname);
				var memStream = new MemoryStream();
				blob.DownloadToStream(memStream);
				return memStream;
			}
			return null;
		}

		public static Replay GetReplay(Guid gameID)
		{
			try
			{
				var memStream = GetReplayAsMemoryStream(gameID);
				return memStream == null ? null : new Replay(gameID, SerializationHelper.Deserialize(memStream));
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public static ErrorCode UploadReplay(MatchReplay replay, Guid gameID)
		{

			try
			{
				Guid replayID = Guid.NewGuid();
				string uniqueBlobName = string.Format("{0}", replayID.ToString());
				CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
				blob.UploadByteArray(SerializationHelper.Serialize(replay));

				Game game = (from g in db.Game
										 where g.Game_ID == gameID
										 select g).FirstOrDefault<Game>();

				game.Replay = uniqueBlobName;
				//if(replay.WinnerTeam == Guid.Empty)
				//{
				//  db.AddToTeams(Team.CreateTeam(replay.WinnerTeam, gameID));
				//}
				db.AddToGameDetails(GameDetails.CreateGameDetails(gameID, (int)replay.Steps, replay.WinnerTeam));
				db.SaveChanges();

				return new ErrorCode(ErrorType.Ok, "Replay has been uploaded.");
			}
			catch (Exception e)
			{
				return new ErrorCode(ErrorType.DataBaseProblems, "Blob problems: \n" + e.ToString());
			}

		}

		public static List<ReplayDescription> GetListOfReplays(Guid accountID)
		{
			try
			{
				List<ReplayDescription> userreplays = new List<ReplayDescription>();

				List<Game> usergames = (from g in db.Game
																where g.Creator_ID == accountID
																select g).ToList<Game>();

				foreach (Game g in usergames)
				{
					List<TeamDescription> teamsingame = new List<TeamDescription>();

					foreach (Team t in (from t in db.Teams
															where t.GameGame_ID == g.Game_ID
															select t))
					{
						TeamDescription tdesc = new TeamDescription();
						tdesc.ID = t.Team_ID;
						tdesc.Intellects = new List<Guid>();

						foreach (Intellect i in t.Intellects)
						{
							tdesc.Intellects.Add(i.Intellect_ID);
						}
					}

					ReplayDescription replay = new ReplayDescription();

					replay.ID = g.Game_ID;
					replay.Name = g.Game_Name;
					replay.Teams = teamsingame;

					userreplays.Add(replay);
				}

				return userreplays;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public static bool UploadFile(String name, String description, String longComment, byte[] data)
		{
			var id = Guid.NewGuid();
			var time = DateTime.UtcNow;
			var uniqueBlobName = String.Format("files/{0}", id);

			db.AddToFiles(File.CreateFile(id, name, time, description, longComment));
			db.SaveChanges();

			CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
			blob.UploadByteArray(data);
			return true;
		}

		public static BlobFile DownloadFile(Guid id)
		{
			var fileInfo = db.Files.FirstOrDefault(f => f.File_Id == id);
			if (fileInfo == null)
			{
				// means that no file
				return null;
			}
			var uniqueBlobName = String.Format("files/{0}", id);
			CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
			// catch here errors
			var data = blob.DownloadByteArray();
			var res = new BlobFile
									{
										ID = fileInfo.File_Id,
										Name = fileInfo.Name,
										CreationTime = fileInfo.CreationTime,
										Description = fileInfo.Description,
										LongDescription = fileInfo.LongComment,
										Data = data
									};
			return res;
		}

		/// <summary>
		/// Gets list of downloadable files in bolb storage
		/// </summary>
		/// <param name="full">Means is in needed show files with duplicated names</param>
		/// <returns>return ONLY structure of file info  without(!) content</returns>
		public static List<BlobFile> GetListOfFiles(bool full = false)
		{
			if (!db.Files.Any())
			{
				return new List<BlobFile>();
			}
			return db.Files.OrderBy("it.CreationTime").ToArray().Select(fileInfo => new BlobFile
																																	{
																																		ID = fileInfo.File_Id,
																																		Name = fileInfo.Name,
																																		CreationTime = fileInfo.CreationTime,
																																		Description = fileInfo.Description,
																																		LongDescription = fileInfo.LongComment,
																																	}).ToList();
		}

		#endregion

		#region QUEUE METHODS

		private static void PutMessage(Message message)
		{
			queue.AddMessage(new CloudQueueMessage(message.ToByteArray()));
		}

		public static Message GetMessage()
		{
			CloudQueueMessage queuemessage = queue.GetMessage();
			if (queuemessage == null)
			{
				return null;
			}

			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream stream = new MemoryStream(queuemessage.AsBytes);
			Message msg = (Message)bf.Deserialize(stream);

			// изменить парсинг сообщения
			//Message msg = ParseMessage(queuemessage);

			// TODO: Пересмотреть удаление сообщений
			queue.DeleteMessage(queuemessage);
			return msg;
		}

		public static int getTasksNumber()
		{
			return queue.RetrieveApproximateMessageCount();
		}

		#endregion

		#region DATABASE METHODS

		#region register & login

        public static bool Register(string accountname, string password, string username, string usersurname, string institution, int course, string email)
		{
			List<Account> test;

			if ((test = (from b in db.Account
									 where b.Account_Name == username
									 select b).ToList<Account>()).Any())
				return false;
			try
			{
				var id = Guid.NewGuid();
				db.AddToAccount(Account.CreateAccount(id, username, password, username, usersurname, institution, course, email));
				db.SaveChanges();
				SetUserRole(RoleType.User, id, DateTime.UtcNow.AddYears(1) /* default one year for user */ /* todo //!! rewrite this !11*/);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool Login(string username, string password)
		{
			return (from b in db.Account
							where b.Account_Name == username && b.Account_Password == password
							select b).Any();
		}

		public static bool ChangePassword(string username, string oldpassword, string newpassword)
		{
			try
			{
				Account user = (from a in db.Account
												where a.Account_Name == username
												select a).FirstOrDefault<Account>();
				if (user.Account_Password != oldpassword)
				{
					return false;
				}
				else
				{
					db.Account.DeleteObject(user);
					user.Account_Password = newpassword;
					db.Account.AddObject(user);
					db.SaveChanges();
					return true;
				}
			}
			catch (Exception e)
			{
				return false;
			}
		}

		#endregion

		#region intellects
		public static List<KeyValuePair<Guid, string>> GetListOfIntellects(Guid userID)
		{
			/*
			Old method realisation: GetListOfIntellects returns list of intellects of logged in user.
			 * 
			List<KeyValuePair<Guid, string>> result = new List<KeyValuePair<Guid, string>>();

			var test = (from b in db.Intellect
									where b.AccountAccount_ID == userID
									select b).ToList<Intellect>();

			foreach (Intellect i in test)
			{
					result.Add(new KeyValuePair<Guid, string> ( i.Intellect_ID, i.Intellect_Name ));
			}

			return result;
			 */

			// Temporary realisation: GetListOfIntellect return list of ALL intellects in the system.
			List<KeyValuePair<Guid, string>> result = new List<KeyValuePair<Guid, string>>();

			var test = (from b in db.Intellect
									where b.AccountAccount_ID == userID
									select b).ToList<Intellect>();

			foreach (Intellect i in test)
			{
				result.Add(new KeyValuePair<Guid, string>(i.Intellect_ID, i.Intellect_Name));
			}

			return result;
		}

		public static bool DeleteIntellect(string name, Guid userID)
		{

			var test = (from b in db.Intellect
									where b.AccountAccount_ID == userID
									select b).ToList<Intellect>();

			var result = test.Where(b => b.Intellect_Name == name).FirstOrDefault<Intellect>();

			if (result != null)
			{
				string neededname = string.Format("intellects/{0}/{1}", userID.ToString(), name);
				CloudBlockBlob blob = container.GetBlockBlobReference(neededname);
				blob.Delete();

				db.Intellect.DeleteObject(result);
				db.SaveChanges();
				return true;
			}
			else
			{
				return false;
			}
		}

        

		#endregion

		#region match

		public static Guid? BeginMatch(List<Guid> intellects, Guid userID, string title, Guid stageId = new Guid())
		{
			Guid gameID = Guid.NewGuid();
			Game match = Game.CreateGame(gameID, userID, DateTime.UtcNow, title);

			var teamInfo = new List<TeamInfo>
											{
												// system command
												//new TeamInfo()
												//  {
												//    TeamId = Guid.Empty
												//  }
											};
			//db.AddToTeams(Team.CreateTeam(Guid.Empty, gameID));
			foreach (Guid id in intellects)
			{
				Team team = Team.CreateTeam(Guid.NewGuid(), gameID);
				var currentIntellect = (from i in db.Intellect
																where i.Intellect_ID == id
																select i).FirstOrDefault<Intellect>();
				team.Intellects.Add(currentIntellect);
				db.AddToTeams(team);
				match.Teams.Add(team);
				teamInfo.Add(new TeamInfo()
											{
												TeamId = team.Team_ID,
												Members = new List<Guid>() { id }
											});
			}

            if(stageId!=Guid.Empty)
            {
                match.Stage = db.Stages.First(x => x.Stage_ID == stageId);
            }

			db.AddToGame(match);

			try
			{
				db.SaveChanges();
			}
			catch (Exception e)
			{
				return null;
			}

			PutMessage(new Message(gameID, teamInfo));

			return gameID;

		}

		public static List<Guid> GetListOfGames(Guid userID)
		{
			List<Game> games = (from b in db.Game
													where b.Creator_ID == userID
													orderby b.CreationTime descending
													select b).ToList<Game>();

			return games.Select(game => game.Game_ID).ToList();
		}

        #region help methods to form Game from Tournament.cs

        public static List<Game> GetListOfStageGames(Guid stageID)
        {
            return (from g in db.Game
                    where g.Stage != null
                    select g).Where(x => x.Stage.Stage_ID == stageID).ToList();
        }

	    public static List<Guid> GetGameIntellects(Guid gameID)
        {
            List<Guid> result = new List<Guid>();

            Game neededGame = (from g in db.Game
                               where g.Game_ID == gameID
                               select g).First<Game>();

            foreach (Intellect i in (from i in db.Intellect
                                     select i))
            {
                /*foreach(Account a in (from a in db.Account
                                      where a.Intellect.Contains(i)
                                      select a))
                {
                    if(a.Game.Contains(neededGame))
                    {
                        result.Add(i.Intellect_ID);
                    }
                }
                 */

                if ((from a in db.Account
                     where a.Account_ID == i.AccountAccount_ID
                     select a).First<Account>().Account_ID == neededGame.Creator_ID)
                    result.Add(i.Intellect_ID);
                
            }

            return result;
        }

        public static Guid GetIntellectOwner(Guid intelectID)
        {
            return (from a in db.Account
                    where a.Account_ID == (from i in db.Intellect
                                           where i.Intellect_ID == intelectID
                                           select i.AccountAccount_ID).First<Guid>()
                    select a.Account_ID).First<Guid>();
        }

        public static DateTime GetGameStartTime(Guid gameID)
        {
            return (from g in db.Game
                    where g.Game_ID == gameID
                    select g.CreationTime).First<DateTime>();
        }

        public static bool DoesMatchHasResult(Guid gameID)
        {
            return (from g in db.Game
                    where g.Game_ID == gameID
                    select g.Replay).FirstOrDefault<string>() != null;
        }

        #endregion
        #endregion

        #region tournaments

        #region tournaments
        public static ErrorCode CreateTournament(string title, DateTime startDate, long maxPlayers, Guid userID)
		{
			if ((from t in db.Tournament
					 where t.Tournament_Name == title && t.Creator_ID == userID
					 select t).ToList<Tournament>().Any<Tournament>())
				return new ErrorCode(ErrorType.WrongInformationInField, "Bad tournament title. User has already created tournament with the same name = " + title);

			if (DateTime.Compare((startDate), DateTime.UtcNow) <= 0)
			{
				return new ErrorCode(ErrorType.WrongInformationInField, "Bad tournament starttime. User's starttime = " + startDate.ToString() + " is early then currenttime = " + DateTime.UtcNow.ToString());
			}

			try
			{
				db.AddToTournament(Tournament.CreateTournament(Guid.NewGuid(), maxPlayers, startDate, userID, title, 0x0, "", ""));
				db.SaveChanges();

				return new ErrorCode(ErrorType.Ok, "Tournament has been created");
			}
			catch (Exception e)
			{
				return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
			}
		}

		public static List<Guid> GetMyTournaments(Guid userID)
		{
			List<Guid> tournamentlist;
			if ((tournamentlist = (from t in db.Tournament
														 where t.Creator_ID == userID
														 select t.Tournament_ID).ToList<Guid>()).Any<Guid>())
			{
				return tournamentlist;
			}
			else
			{
				// User didn't create any tournaments.
				return null;
			}

		}

        public static ErrorCode DeleteTournament(Guid tournamentID, Guid userID)
        {
            Tournament needed;

            if ((needed = (from t in db.Tournament
                           where t.Creator_ID == userID && t.Tournament_ID == tournamentID
                           select t).FirstOrDefault<Tournament>()) != null)
            {
                try
                {

                    db.Tournament.DeleteObject(needed);

                    db.SaveChanges();

                    return new ErrorCode(ErrorType.Ok,
                                         "Tournament with title " + needed.Tournament_Name + " was deleted.");
                }
                catch (Exception e)
                {
                    return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
                }
            }
            else
                return new ErrorCode(ErrorType.WrongInformationInField, "User " + (from u in db.Account
                                                                                   where u.Account_ID == userID
                                                                                   select u.Account_Name) +
                                                                        " didn't create that tournament. ");

        }

	    public static List<Guid> GetAvailableTournaments(Guid userID)
		{
			List<Tournament> actualtournaments;

			if ((actualtournaments = (from t in db.Tournament
																where DateTime.Compare((t.StartTime), DateTime.UtcNow) >= 0
																select t).ToList<Tournament>()).Any<Tournament>())
			{
				Account user = (from a in db.Account
												where a.Account_ID == userID
												select a).FirstOrDefault<Account>();

				List<Guid> recommendedtournaments = new List<Guid>();

				foreach (Tournament t in actualtournaments)
				{
					if (!user.TournamentPlayer.Contains(t))
					{
						recommendedtournaments.Add(t.Tournament_ID);
					}
				}

				if (recommendedtournaments.Any<Guid>())
					return recommendedtournaments;
				else
					// User competes in all actual tournaments.
					return null;
			}

			else
				// There are no actual tournaments.
				return null;

		}

		public static List<Tournament> GetActiveTournaments()
		{
            return (from t in db.Tournament
                    where t.State_Code != (int)State.Finished
                    select t).ToList();
		}

        public static List<Stage> GetTournamentStages(Guid tournamentID)
        {
            return (from s in db.Stages
             where s.Tournament.Tournament_ID == tournamentID
             select s).ToList();
        }
		
		public static void UpdateTournament(Guid tournamentID, State newState)
		{
			Tournament neededTournament;
			
			if ((neededTournament = (from t in db.Tournament
									where t.Tournament_ID == tournamentID
									select t).FirstOrDefault<Tournament>()) != null)
			{
				try
				{
					neededTournament.State_Code = (int)newState;
					db.SaveChanges();
				}
				catch (Exception e)
				{
					throw e;
				}
			}
			else
			{
				throw new ArgumentException("No stage with that ID: " + tournamentID.ToString());
			}
		}

        public static ErrorCode JoinTournament(Guid tournamentID, Guid userID)
        {

            Tournament neededtournament = (from t in db.Tournament
                                           where t.Tournament_ID == tournamentID
                                           select t).FirstOrDefault<Tournament>();



            if (neededtournament != null)
            {

                Account updatedaccount = (from a in db.Account
                                          where a.Account_ID == userID
                                          select a).FirstOrDefault<Account>();

                if (updatedaccount.Intellect.Count == 0)
                    return new ErrorCode(ErrorType.IllegalDll, "User " + (from u in db.Account
                                                                          where u.Account_ID == userID
                                                                          select u.Account_Name) +
                                                               " doesn't have intellect.");

                if (updatedaccount.TournamentPlayer.Contains(neededtournament))
                {
                    return new ErrorCode(ErrorType.WrongInformationInField, "User already competes in that tournament");
                }
                else
                {
                    try
                    {
                        neededtournament.Stages.First().Intellects.Add(updatedaccount.Intellect.Last());
                        updatedaccount.TournamentPlayer.Add(neededtournament);
                        db.SaveChanges();

                        return new ErrorCode(ErrorType.Ok, "User " + (from u in db.Account
                                                                      where u.Account_ID == userID
                                                                      select u.Account_Name) + " joined to tournament.");

                    }
                    catch (Exception e)
                    {
                        return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: \n" + e.ToString());
                    }
                }

            }
            else
            {
                return new ErrorCode(ErrorType.WrongInformationInField, "There are no tournaments with that ID.");
            }

        }

	    public static ErrorCode LeaveTournament(Guid tournamentID, Guid userID)
		{

			Tournament neededtournament = (from t in db.Tournament
																		 where t.Tournament_ID == tournamentID
																		 select t).FirstOrDefault<Tournament>();

			if (neededtournament != null)
			{

				Account updatedaccount = (from a in db.Account
																	where a.Account_ID == userID
																	select a).FirstOrDefault<Account>();

				if (updatedaccount.TournamentPlayer.Contains(neededtournament))
				{
					try
					{
						updatedaccount.TournamentPlayer.Remove(neededtournament);

						// todo clean intellects from stages
						db.SaveChanges();

						return new ErrorCode(ErrorType.Ok, "User " + updatedaccount.Account_Name + " leaved the tournament.");
					}
					catch (Exception e)
					{
						return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
					}
				}
				else
				{
					return new ErrorCode(ErrorType.WrongInformationInField, "User doesn't compete that tournament");
				}

			}
			else
				return new ErrorCode(ErrorType.WrongInformationInField, "There are no tournaments with that ID.");

		}

        public static Tournament GetTournament(Guid tournamentID)
        {
            return (from t in db.Tournament
                    where t.Tournament_ID == tournamentID
                    select t).First<Tournament>();
        }

        #endregion

        #region stage

        public static ErrorCode AddStage(Guid tournamentID, DateTime startTime, State state = State.NotStarted , string type = "TO DO: Какие типы")
        {
            Tournament currentTournament;
            if ((currentTournament = (from t in db.Tournament
                                                 where t.Tournament_ID == tournamentID
                                                 select t).FirstOrDefault<Tournament>()) != null)
            {
                if (DateTime.Compare(startTime, DateTime.Now) > 0)
                {
                    try
                    {
                        Stage stage = Stage.CreateStage(Guid.NewGuid(), (int) state, type, startTime);
                        stage.Tournament = currentTournament;
                        db.AddToStages(stage);
                        db.SaveChanges();

                        return new ErrorCode(ErrorType.Ok, "New stage has been created.");
                    }
                    catch (Exception e)
                    {
                        return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
                    }
                }
                else
                {
                    return new ErrorCode(ErrorType.DataBaseProblems, "Start time " + startTime.ToString() + " is past. Current time is " + DateTime.Now.ToString());
                }
            }
            else
            {
                return new ErrorCode(ErrorType.DataBaseProblems, "No tournaments with that ID: " + tournamentID.ToString());
            }

        }

        public static ErrorCode AddScore(Stage stage, Account account, int value)
        {
            try
            {
                Score score = Score.CreateScore(Guid.NewGuid(), stage.Stage_ID, account.Account_ID, value);
                score.Account = account;
                score.Stage = stage;
                db.AddToScores(score);
                db.SaveChanges();

                return new ErrorCode(ErrorType.Ok, "New stage has been created.");
            }
            catch (Exception e)
            {
                return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
            }
        }



	    public static ErrorCode DeleteStage(Guid stageID)
        {
            Stage neededStage;

            if ((neededStage = (from s in db.Stages
                                where s.Stage_ID == stageID
                                select s).FirstOrDefault<Stage>()) != null)
            {
                try
                {
                    db.Stages.DeleteObject(neededStage);
                    db.SaveChanges();

                    return new ErrorCode(ErrorType.Ok, "Stage with id " + stageID.ToString() + " was deleted.");
                }
                catch (Exception e)
                {
                    return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
                }
            }
            else
            {
                return new ErrorCode(ErrorType.DataBaseProblems, "No stage with that ID: " + stageID.ToString());
            }

        }

        public static void UpdateStage(Guid stageID, State newState)
        {
            Stage neededState;
            if ((neededState = (from s in db.Stages
                                where s.Stage_ID == stageID
                                select s).FirstOrDefault<Stage>()) != null)
            {
                try
                {
                    neededState.State_Code = (int)newState;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new ArgumentException("No stage with that ID: " + stageID.ToString());
            }

        }

		
		
        #endregion

        #endregion


        #region roles

        public static DateTime UserRoleValidUntil(Guid userID, RoleType roleCode)
		{
			var needed = (from r in db.UserRole
										where r.AccountAccount_ID == userID && r.Role_Code == (int)roleCode
										select r).ToList();
			return needed.Any() ? needed.First().Until : DateTime.UtcNow.AddYears(-1) /* date in past*/;
		}

		public static bool IsUserAdmin(Guid userID)
		{
			return IsUser(userID, RoleType.Admin);
		}

		/// <summary>
		/// Check if user with 'userID' have role 'roleCode'
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="roleCode"></param>
		/// <returns></returns>
		public static bool IsUser(Guid userID, RoleType roleCode)
		{
			return DateTime.Compare(UserRoleValidUntil(userID, roleCode), DateTime.UtcNow) >= 0;
		}

		public static ErrorCode SetUserRole(RoleType rolecode, Guid userID, DateTime until)
		{
			List<UserRole> thatRoleofUser = (from r in db.UserRole
																			 where r.AccountAccount_ID == userID && r.Role_Code == (int)rolecode
																			 select r).ToList<UserRole>();

			if (thatRoleofUser.Any())
			{
				if (DateTime.Compare((until), DateTime.UtcNow) <= 0)
				{
					return new ErrorCode(ErrorType.WrongInformationInField, "Datetime is in past");
				}
				else
				{
					try
					{

						db.UserRole.DeleteObject(thatRoleofUser.First<UserRole>());
						db.UserRole.AddObject(UserRole.CreateUserRole(Guid.NewGuid(), until, userID, (int)rolecode));
						db.SaveChanges();
						return new ErrorCode(ErrorType.Ok, "Role has been given.");

					}
					catch (Exception e)
					{
						return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
					}

				}
			}
			else
			{
				try
				{
					db.UserRole.AddObject(UserRole.CreateUserRole(Guid.NewGuid(), until, userID, (int)rolecode));

					db.SaveChanges();

					return new ErrorCode(ErrorType.Ok, "Role has been given.");
				}
				catch (Exception e)
				{
					return new ErrorCode(ErrorType.DataBaseProblems, "Database problems: " + e.ToString());
				}
			}

		}

		public static string[] GetUserRoles(Guid userID)
		{

			return (from r in db.UserRole
							where r.AccountAccount_ID == userID
							select r.Role_Code).ToList().
										 Select(x =>
												 Enum.GetName(typeof(RoleType), (RoleType)x)).ToArray();
		}

		#endregion

		#region security
		public static List<string> GetListOfIllegalReferences()
		{
			return (from s in db.Securities
							select s.IllegalReferenceName).ToList<string>();
		}
		#endregion

		#endregion





        
    }
}

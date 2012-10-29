﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using WarSpot.Cloud.Storage.Account;
using System.Diagnostics;


namespace WarSpot.Cloud.Storage.Account
{
	public class AccountDataSource
	{
		private static CloudStorageAccount storageAccount;
		private AccountDataContext context;

		static AccountDataSource()
		{
			//CloudStorageAccount.SetConfigurationSettingPublisher(
			//          (a, b) => b(RoleEnvironment.GetConfigurationSettingValue(a)));

			storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

			CloudTableClient.CreateTablesFromModel(
			typeof(AccountDataContext),
			storageAccount.TableEndpoint.AbsoluteUri,
			storageAccount.Credentials);
		}

		public AccountDataSource()
		{
			this.context = new AccountDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
			this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
		}


		public bool AddAccountEntry(AccountEntry newItem) // создаем новый аккаунт
		{
			var results = (from g in this.context.AccountEntry select g).ToList(); // посмотрим, что у нас уже лежит
			var entry = results.Where(g => g.Name == newItem.Name).FirstOrDefault<AccountEntry>(); // посмотрим, нет ли аккаунта с таким же
			// именем

			if (entry != null) // если есть
			{
				return false; // false!!!
			}

			else // если нет
			{
				this.context.AddObject("AccountEntry", newItem); // создаем новую запись в тамблице
				this.context.SaveChanges();
				return true;
			}
		}

		public bool CheckAccountEntry(string username, string pass) // проверяем на совпадение имени\пароля
		{
			var results = (from g in this.context.AccountEntry select g).ToList();
			var entry = results.Where(g => g.Name == username).FirstOrDefault<AccountEntry>();

			if (entry != null) // нашли аккаунт с нужным именем
			{
				if (entry.Pass == pass) // проверяем правильность пароля
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else // не нашли аккаунт с нужным именем
			{
				return false;
			}

		}



	}
}
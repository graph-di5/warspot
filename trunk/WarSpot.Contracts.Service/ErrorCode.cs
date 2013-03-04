using System.Runtime.Serialization;

namespace WarSpot.Contracts.Service
{
	/// <summary>
	/// Error codes
	/// </summary>
	public enum ErrorType
	{
		/// <summary>
		/// All done fine.
		/// </summary>
		Ok,

		/// <summary>
		/// Connection exceptions
		/// </summary>
		WrongLoginOrPassword,

        NotLoggedIn,

		#region .dll file exceptions
		/// <summary>
		/// Not a valid .NET dll
		/// </summary>
		BadFileType,

		/// <summary>
		/// Multi-threading, reflection
		/// </summary>
		ForbiddenUsages,	

        BadFileName,
		#endregion

        DataBaseProblems,

        WrongInformationInField,

		/// <summary>
		/// Some error, that not listed before.
		/// </summary>
        UnknownException,

        #region Security
            IllegalReference,
        #endregion
    }

	/// <summary>
	/// Class is needed for reporting errors from UserService to the client
	/// </summary>
	[DataContract]
	public class ErrorCode
	{
		/// <summary>
		/// Error code.
		/// </summary>
		[DataMember]
		public ErrorType Type { get; private set; }

		/// <summary>
		/// Some string description of the error.
		/// </summary>
		[DataMember]
		public string Message { get; private set; }

		public ErrorCode(ErrorType type = ErrorType.Ok, string message = "")
		{
			Type = type;
			Message = message;
		}

        public override string ToString()
        {
            return this.Type.ToString() + this.Message;
        }
	}

}

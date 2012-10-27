using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Contracts.Service
{
	public class ErrorCode
	{
		public ErrorType Type {get; private set;}
		public string Message {get; private set;}
		
		public ErrorCode(ErrorType type)
		{
			Type = type;
			Message = "";
		}

		public ErrorCode(ErrorType type, string message)
		{
			Type = type;
			Message = message;
		}
	}

	public enum ErrorType
	{
		Ok,
		//.dll file exceptions
		BadFileType,
		ForbiddenUsages, //Multi-threading, reflection
		//other
		UnknownException
	}
}

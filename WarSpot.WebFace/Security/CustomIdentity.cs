﻿using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using WarSpot.Cloud.Storage;
using WarSpot.Common.Utils;

namespace WarSpot.WebFace.Security
{
	public class CustomIdentity : ICustomIdentity
	{
		/// <summary>
		/// Authenticate and get identity out with roles
		/// </summary>
		/// <param name="userName">User name</param>
		/// <param name="password">Password</param>
		/// <returns>Instance of identity</returns>
		public static CustomIdentity GetCustomIdentity(string userName, string password)
		{
			CustomIdentity identity = new CustomIdentity();
			if (Warehouse.Login(userName, HashHelper.GetMd5Hash(password)))
			{
				identity.IsAuthenticated = true;
				identity.Name = userName;
				identity.Id = (from b in Warehouse.db.Account
											 where b.Account_Name == userName
											 select b).First().Account_ID;
				identity.Roles = Warehouse.GetUserRoles(identity.Id);
				return identity;
			}
			return identity;
		}

		private CustomIdentity() { }

		public string AuthenticationType
		{
			get { return "Custom"; }
		}

		public Guid Id { get; private set; }

		public bool IsAuthenticated { get; private set; }

		public string Name { get; private set; }

		private string[] Roles { get; set; }

		public bool IsInRole(string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				throw new ArgumentException("Role is null");
			}
			return Roles.Where(one => one.ToUpper().Trim() == role.ToUpper().Trim()).Any();
		}

		/// <summary>
		/// Create serialized string for storing in a cookie
		/// </summary>
		/// <returns>String representation of identity</returns>
		public string ToJson()
		{
			string returnValue = string.Empty;
			IdentityRepresentation representation = new IdentityRepresentation()
			{
				IsAuthenticated = this.IsAuthenticated,
				Name = this.Name,
				Roles = string.Join("|", this.Roles),
				Id = this.Id.ToString()
			};
			DataContractJsonSerializer jsonSerializer =
					new DataContractJsonSerializer(typeof(IdentityRepresentation));
			using (MemoryStream stream = new MemoryStream())
			{
				jsonSerializer.WriteObject(stream, representation);
				stream.Flush();
				byte[] json = stream.ToArray();
				returnValue = Encoding.UTF8.GetString(json, 0, json.Length);
			}

			return returnValue;
		}

		/// <summary>
		/// Create identity from a cookie data
		/// </summary>
		/// <param name="cookieString">String stored in cookie, created via ToJson method</param>
		/// <returns>Instance of identity</returns>
		public static ICustomIdentity FromJson(string cookieString)
		{

			IdentityRepresentation serializedIdentity = null;
			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(cookieString)))
			{
				DataContractJsonSerializer jsonSerializer =
						new DataContractJsonSerializer(typeof(IdentityRepresentation));
				serializedIdentity = jsonSerializer.ReadObject(stream) as IdentityRepresentation;
			}
			CustomIdentity identity = new CustomIdentity()
			{
				IsAuthenticated = serializedIdentity.IsAuthenticated,
				Name = serializedIdentity.Name,
				Roles = serializedIdentity.Roles
						.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries),
				Id = Guid.Parse(serializedIdentity.Id)
			};
			return identity;
		}

	}
}
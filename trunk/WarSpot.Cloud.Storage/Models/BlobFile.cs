using System;
using System.ComponentModel.DataAnnotations;

namespace WarSpot.Cloud.Storage.Models
{
	public class BlobFile
	{
		public Guid ID;
		[Display(Name = "Имя файла (как он виден пользователям)")]
		public string Name;
		[Display(Name = "Время загрузки")]
		public DateTime CreationTime;
		[Display(Name = "Краткое описание")]
		public string Description;
		[Display(Name = "Полное описание")]
		public string LongDescription;
		[Display(Name = "Файл")]
		public byte[] Data;
	}
}

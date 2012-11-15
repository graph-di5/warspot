using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace SerializeData//Приличное простое пособие: http://code-inside.net/serialization/
{
	[Serializable]//Нужно писать перед классом, если будем сериализовать.
	class Data
	{
		public int Number { set; get; }
		public string Name { set; get; }

		public Data(int number, string name) { Number = number; Name = name; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			List<Data> _serialazedList = new List<Data>();//Пишем всё сюда, потом сериализуем
			List<Data> _deserialazedList = new List<Data>();//Для проверки при десериализации

			_serialazedList.Add(new Data(1, "first"));
			_serialazedList.Add(new Data(2, "second"));
			_serialazedList.Add(new Data(3, "third"));

			//откроем поток для записи в файл
			FileStream fs = new FileStream("file.s", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			BinaryFormatter bf = new BinaryFormatter();

			bf.Serialize(fs, _serialazedList);
			fs.Close();


			fs = new FileStream("file.s", FileMode.Open, FileAccess.Read, FileShare.Read); //просто не создаём новый поток, а работаем со старым (совсем неважно)
			bf = new BinaryFormatter();//то же

			//Вот и десериализация
			_deserialazedList = (List<Data>)bf.Deserialize(fs);

			fs.Close();

			foreach (Data _data in _deserialazedList)//смотрим, что там наполучалось
			{
				Console.WriteLine("Number: {0}, Name: {1}", _data.Number, _data.Name);
			}

			Console.ReadKey();

		}

	}
}
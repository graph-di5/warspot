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
	
		public Data(){ Number = 0; Name = "0"; }//Конструктор без аргументов нужен для наследования
	}

	[Serializable]
	class AdvancedData : Data
	{
		public int Age { set; get; }

		public AdvancedData(int number, string name, int age) { Number = number; Name = name; Age = age; }

		public AdvancedData() { Number = 0; Name = "0"; Age = 0; }

	}

	[Serializable]
	class AnotherAdvancedData: Data
	{
		public int Weight { set; get; }

		public AnotherAdvancedData(int number, string name, int weight) { Number = number; Name = name; Weight = weight; }
	}

	[Serializable]
	class DeeperAdvancedData : AdvancedData
	{
		public int Height { set; get; }

		public DeeperAdvancedData(int number, string name, int age, int height) { Number = number; Name = name; Age = age; Height = height; }
	}



	class Program
	{
		static void Main(string[] args)
		{
			List<Data> _serialazedList = new List<Data>();//Пишем всё сюда, потом сериализуем
			List<Data> _deserialazedList = new List<Data>();//Для проверки при десериализации

			
			_serialazedList.Add(new Data(1, "first"));
			_serialazedList.Add(new AdvancedData(2, "second", 1));
			_serialazedList.Add(new AnotherAdvancedData(3, "third", 2));
			_serialazedList.Add(new DeeperAdvancedData(4, "fourth", 1, 3));
			

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
				

				if (_data is DeeperAdvancedData)//Сперва проверяем потомков, потом родителей!
				{
					var _forDisplayOnly3 = _data as DeeperAdvancedData;

					Console.WriteLine("Number: {0}, Name: {1}, Age: {2}, Height: {3}", _forDisplayOnly3.Number, _forDisplayOnly3.Name, _forDisplayOnly3.Age, _forDisplayOnly3.Height);
				}

				else if (_data is AnotherAdvancedData)
				{
					var _forDisplayOnly2 = _data as AnotherAdvancedData;

					Console.WriteLine("Number: {0}, Name: {1},  Weight: {2}", _forDisplayOnly2.Number, _forDisplayOnly2.Name, _forDisplayOnly2.Weight);
				}

				else if (_data is AdvancedData)
				{
					var _forDisplayOnly1 = _data as AdvancedData;
					Console.WriteLine("Number: {0}, Name: {1}, Age: {2}", _forDisplayOnly1.Number, _forDisplayOnly1.Name, _forDisplayOnly1.Age);
				}


				else
				{
					Console.WriteLine("Number: {0}, Name: {1}", _data.Number, _data.Name);
				}
			}

			Console.ReadKey();

		}

	}
}
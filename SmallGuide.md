#summary Small guide for end users: writing intellect.

## Введение ##

Этот документ описывает процесс создания простого интеллекта существа для игры WarSpot.

В разделе [Файлов](http://warspot.cloudapp.net/File) доступен актуальный готовый пример интеллекта, его достаточно лишь скомпилировать и загрузить на серверы WarSpot.

## Подготовка ##

### Установка ###
Для начала Вам надо поставить Microsoft Visual Studio 2010/2012 с пакетом разработки для .NET 4.0.

> Вы можете использовать другие средства разработки и компиляции .NET библиотеки **на свой страх и риск**, но официально такие способы разработки не поддерживаются.

### Создание проекта ###
Далее Вы можете или использовать скаченный пример, или создать проект сами, для этого:
  1. Создайте в Visual Studio новый проект (.NET 4.0, Class Library)
  1. Добавьте в Reference библитеку WarSpot.Contracts.Intellect, доступную в разделе [Файлов](http://warspot.cloudapp.net/File)

### Подготовка к отладке ###
Если Вы создаете интеллект с нуля, то для **удобства отладки** выполните следующие шаги :
  1. Скачайте WarSpot.ConsoleComputer (уже включен в поставку примера интеллекта).
  1. Откройте свойства проекта-библиотеки
  1. В разделе Debug в пункте  Start external program укажите полный путь к WarSpot.ConsoleComputer.exe
  1. В пункте Command line arguments укажите **не менее двух** библиотек интерфейсов, простейщий вариант - укажите два раза свою библиотеку

После выполнения всех этих действий вы легко сможете отлаживать свой класс существа, расставляя точки останова (Breakpoints) и запуская проект через Debug > Start Debugging.

## Правила ##

Полные правила доступны в разделе [Rules](Rules.md).
Вы должны предоставить скомпилированную dll - библиотеку содержащую класс существа.
Существа оказываются в некотором мире. Мир дискретный как по координатам так и по времени. Вашему существу доступны несколько действий и некоторая видимая площадь мира.

## Код и запуск ##
  1. Создайте класс и унаследуйте его от интерфейса [IBeingInterface](http://code.google.com/p/warspot/source/browse/trunk/WarSpot.Contracts.Intellect/IBeingInterface.cs)
  1. Создайте два метода Contruct и Think с сигнатурами, соответствующими интерфейсу IBeingInterface
  1. Реализуйте оба метода согласно вашим идеям. Ниже приведен пример реализации нескольких вспомогательных функций
  1. При необходимости создайте дополнительные методы и поля класса
  1. Скомпилируйте и запустите
  1. Проанализируйте запись матча (для этого можно использовать одно из нескольких визуальных приложений)
  1. Внесите необходимые правки в алгоритм
  1. Если Вы выполнили шаги из раздела **Подготовка к отладке** или использовали пример существа, то поставьте точки останова во всех необходимых местах вашего кода.
  1. Если Вы считаете, что Ваше существо готово к боям, то загружайте его на сервер через сайт WarSpot.

### Примеры вспомогательных функций ###

#### Создаем класс, наследуем его от IBeingInterface ####
```
class Program : IBeingInterface
    {
        public BeingCharacteristics Construct(ulong step, float ci)
        {
            throw new NotImplementedException();
        }

        public GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            throw new NotImplementedException();
        }
    }
```

#### Заполняем метод Construct ####
```
/// <summary>
/// This method is called on every being creation. Your intellect should distribute Ci to being abilities.
/// </summary>
/// <param name="step">Step of the match</param>
/// <param name="ci">Ci(Энергия) available for the constructing being.</param>
/// <returns>Desired characteristic of the future being.</returns>
public BeingCharacteristics Construct(ulong step, float ci)
{
	//Being characteristics may be different. It's you to decide. But remember about the cost.
	return new BeingCharacteristics((ci - 13.0f) / 0.8f, 3.0f, 4);
}
```

#### Заполняем метод Think ####
В этом примере проводится проверка на количество здоровья у существа и в зависимости от результата запускает либо метод по самолечению (selfHeal), либо метод по созданию потомства (GameActionMakeOffspring), либо метод по поиску пищи (goEat). Позже будут приведены примеры реализации этих методов. Разумеется, это только пример, Вы можете использовать свои методы вдобавок, или взамен указанным.
```
/// <summary>
/// Main function of the being. Here your code must make decision what the being shoud do.
/// </summary>
/// <param name="step">Current game step</param>
/// <param name="characteristics">Currnet characteristics of this being.</param>
/// <param name="area">Information about world in the visiable area.</param>
/// <returns>GameAction that represents decision.</returns>
public GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
{			
	// Here you can place a lot of decesions 
	if (characteristics.Health < characteristics.MaxHealth && characteristics.Ci > characteristics.MaxHealth * 0.6f)
	{
		return selfHeal(characteristics);
	}
	else if (characteristics.Ci > 13.0f + characteristics.MaxHealth * 0.8f)
	{
		return new GameActionMakeOffspring(characteristics.Id, 13.0f + characteristics.MaxHealth * 0.8f);
	}
	else
	{
		return goEat(characteristics, area);
	}
}
```

#### Реализация метода лечения ####
```
/// <summary>
/// Heal itself
/// </summary>
///  <param name="characteristics">Being characteristics</param>
/// <returns>Return computed action</returns>
private GameAction selfHeal(BeingCharacteristics characteristics)
{
	float _ci = 0.0f;
	if (characteristics.Ci >= (characteristics.MaxHealth - characteristics.Health) * 3.0f)
	{//Heal itself fully if it can
		_ci = (characteristics.MaxHealth - characteristics.Health) * 3.0f;

		if (_ci > characteristics.Ci * 0.6f)
		{//If more than one permitted then reduces
			_ci = characteristics.Ci * 0.6f;
		}
	}
	else
	{
		_ci = Math.Abs(characteristics.Ci);
	}

	return new GameActionTreat(characteristics.Id, 0, 0, _ci);//In relative coordinates
}
```
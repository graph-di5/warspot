# Руководство #

#1 Создаем класс, наследуем его от IBeingInterface
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

#2 Заполняем метод Construct
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

#3 Заполняем метод Think
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

#4 Реализация метода лечения
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
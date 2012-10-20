using System.Runtime.Serialization;
using System.ServiceModel;
using Microsoft.WindowsAzure.StorageClient;

namespace WarSpot.Cloud.UserService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {

		[OperationContract]
		void addMsg(CloudQueueMessage msg);

        // TODO: Добавьте здесь операции служб
    }
}

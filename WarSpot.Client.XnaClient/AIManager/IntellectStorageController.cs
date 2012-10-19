using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Client.XnaClient.AIManager
{
    static class IntellectStorageController
    {
        private static List<Intellect> _intellects = new List<Intellect>();

        // Относительынй путь к папке на клиентском компрьютере, в которой будут хранится локально интеллекты пользователя
        private static string _localPath;

        public static void UpdateIntellects()
        {
            // Заполнение листа интеллектов с сервера
        }

        public static void AddIntellect(/* AI name */)
        {
            // Добавляет интеллект в лист и на сервер, если проходит проверку
        }

        public static void DeleteIntellect(/* AI name */)
        {
          
        }

        public static void RenameIntellect(/* AI name, string newName */)
        {
        }

        public static List<string> GetIntellectsNames()
        {
            List<string> tmp = new List<string>();

            foreach (var ai in _intellects)
            {
                tmp.Add(ai.Name);
            }

            return tmp;
        }
    }
}

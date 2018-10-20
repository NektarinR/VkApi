using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Tochka
{
    public class Vk : ISocial
    {
        private readonly VkApi _vkapi;
        private readonly ulong _appId = 6724571;
        private string user;

        public Vk()
        {
            _vkapi = new VkApi();
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        public void Auth(string login, string password)
        {
            _vkapi.Authorize(new ApiAuthParams
            {
                ApplicationId = _appId,
                Login = login,
                Password = password,
                Settings = Settings.Wall
            });
            
        }

        /// <summary>
        /// Преобразование короткого имени (screen_name) в идентификатор
        /// </summary>
        /// <param name="name">Короткое имя пользователя или группы</param>
        /// <returns>Идентификатор пользователя или группы</returns>
        private long? ConvertShortNameToId(string name)
        {
            var userGroup = _vkapi.Utils.ResolveScreenName(name);
            if (userGroup == null || userGroup.Type == VkNet.Enums.VkObjectType.Application)
                return null;
            return userGroup.Type != VkNet.Enums.VkObjectType.User ? -userGroup.Id : userGroup.Id;
        }

        /// <summary>
        /// Получение постов со стены пользователя или группы
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="count">Количество постов, которые надо получить</param>
        /// <returns></returns>
        public ICollection<string> GetPosts(string name, int count)
        {
            ICollection<string> result = null;
            var id = ConvertShortNameToId(name);
            if (id == null)
                return result;
            result = new List<string>();
            var timeRes = _vkapi.Wall.Get(new WallGetParams
            {
                OwnerId = id,
                Count = (ulong)count
            });
            if (timeRes?.WallPosts != null)
            {
                foreach (var t in timeRes.WallPosts)
                    result.Add(t.Text);
                user = name;
            }            
            return result;
        }

        /// <summary>
        /// Запостить данные у себя на странице
        /// </summary>
        /// <param name="data">Данные, которые надо расположить у себя на стене</param>
        public void SendPostToMe(string data)
        {
            _vkapi.Wall.Post(new WallPostParams()
            {
                OwnerId = _vkapi.UserId,
                Message = $"{user}, статистика для последних 5 постов:\n{data}"
            });
        }
    }
}

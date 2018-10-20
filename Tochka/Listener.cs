using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Exception;

namespace Tochka
{
    public class Listener 
    {

        /*
         Схема работы:
          Сначала проверяется авторизован ли пользователь, если нет, то происходит авторизация
          Если авторизация не успешна, то еще раз пытается авторизоваться.
          Если авторизация успешна, то надо ввести имя группы или пользователя, с которого будут браться посты, 
            либо можно сменить пользователя командой -1
          Если ошибки в идентификаторе (имя пользователя или группы), введеном пользователем, нет, 
            то считывается 5 постов и отправляются на стену пользователя
        */

        private readonly ISocial _vk;
        private readonly ITextAnalyze _analyze;
        
        private string template = "йцукенгшщзхъфывапролджэячсмитьбю";
        private bool isSignIn = false;
        public Listener()
        {
            _vk = new Vk();
            _analyze = new MyAnalyze();
        }

        public void Start()
        {
            while (true)
            {
                if (!isSignIn)
                    SignIn();
                else
                {
                    Console.Write("Введите имя пользователя или группы (для смена логина введите -1): ");
                    var id = Console.ReadLine().Trim();
                    if (id == "-1")
                    {
                        isSignIn = false;
                        continue;
                    }
                    else if (id != "")
                    {
                        GetPosts(id);
                    }
                }
            }
        }

        private void GetPosts(string id)
        {
            try
            {
                var posts = _vk.GetPosts(id, 5);
                if (posts != null && posts.Count > 0)
                {
                    var dict = _analyze.FindFrequency(posts, template);
                    var jsonStr = JsonConvert.SerializeObject(dict);
                    Console.WriteLine($"{id}, статистика для последних 5 постов:{jsonStr}");
                    Console.WriteLine("Публикация данных на вашей стене...");
                    _vk.SendPostToMe(jsonStr);
                    Console.WriteLine("Опубликовано");
                }
                else
                {
                    Console.WriteLine("Посты не найдены.");
                }
            }
            catch (UserAuthorizationFailException)
            {
                Console.WriteLine("У вас нет прав на выполнение этой операции");
            }
            catch (VkApiException)
            {
                Console.WriteLine("Ошибка в доступе");
            }
        }

        private void SignIn()
        {
            try
            {
                Console.Write("Введите логин: ");
                var login = Console.ReadLine().Trim();
                Console.Write("Введите пароль: ");
                var pass = Console.ReadLine().Trim();
                Console.WriteLine("Осуществляется вход...");
                _vk.Auth(login, pass);
                Console.WriteLine();
                Console.WriteLine("Упешная авторизация");
                isSignIn = true;
            }
            catch (VkApiException)
            {
                Console.WriteLine("Ошибка входа.");
            }
            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка");
            }
        }
    }
}

using System.Collections;
using Domain.Catalog.Entities;
using Domain.Identity.Entities;
using Domain.Templates;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public static class DatabaseInitialization
{
    public static async Task InitData(OnlineEventContext context, IPasswordHasher? passwordHasher, string? password)
    {
        await context.Database.EnsureCreatedAsync();
        if (!await context.Users.AnyAsync())
        {
            const string email = "a.y.senokosov@student.khai.edu";

            var user = context.Users
                .SingleOrDefault(u => u.Email == email && !u.IsDeleted);

            if (user == null && passwordHasher != null && password != null)
            {
                var salt = Guid.NewGuid().ToByteArray();
                {
                    var person = new User()
                    {
                        FirstName = "Anton",
                        LastName = "Senokosov",
                        Email = email,
                        Hash = passwordHasher.Hash(password, salt),
                        Salt = salt,
                        IsAdmin = true,
                        IsSuperAdmin = true
                    };

                    await context.Users.AddAsync(person);
                    await context.SaveChangesAsync();
                }
            }
        }

        if (!await context.TypeOfEvents.AnyAsync())
        {
            await context.TypeOfEvents.AddRangeAsync(GetTypes());
            await context.SaveChangesAsync();
        }

        if (!await context.MeetingPlatforms.AnyAsync())
        {
            await context.MeetingPlatforms.AddRangeAsync(GetPlatforms());
            await context.SaveChangesAsync();
        }

        if (!await context.Speakers.AnyAsync())
        {
            await context.Speakers.AddRangeAsync(GetSpeakers());
            await context.SaveChangesAsync();
        }

        if (!await context.OnlineEvents.AnyAsync())
        {
            await context.OnlineEvents.AddRangeAsync(GetEvents());
            await context.SaveChangesAsync();
        }

        if (!await context.MailTemplates.AnyAsync())
        {
            await context.MailTemplates.AddRangeAsync(GetMailTemplates());
            await context.SaveChangesAsync();
        }

        if (!await context.EventPlatforms.AnyAsync())
        {
            await context.EventPlatforms.AddRangeAsync(GetEventPlatforms());
            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<EventType> GetTypes()
    {
        return new List<EventType>()
        {
            new EventType()
            {
                Name = "Вебінар"
            },
            new EventType()
            {
                Name = "Семінар"
            },
            new EventType()
            {
                Name = "Курс"
            }
        };
    }

    private static IEnumerable<MeetingPlatform> GetPlatforms()
    {
        return new List<MeetingPlatform>()
        {
            new MeetingPlatform()
            {
                Name = "Zoom",
                Url = "https://zoom.us"
            },
            new MeetingPlatform()
            {
                Name = "Microsoft Teams",
                Url = "https://www.microsoft.com/en/microsoft-teams/group-chat-software"
            },
            new MeetingPlatform()
            {
                Name = "Google Meet",
                Url = "https://meet.google.com"
            },
            new MeetingPlatform()
            {
                Name = "Skype",
                Url = "https://www.skype.com"
            },
            new MeetingPlatform()
            {
                Name = "Discord",
                Url = "https://discord.com"
            }
        };
    }

    private static IEnumerable<OnlineEvent> GetEvents()
    {
        return new List<OnlineEvent>()
        {
            new OnlineEvent()
            {
                Name = "Розвиток критичного мислення: що це, кому, для чого і як?",
                TypeId = 1,
                Description = "Test",
                AboutEvent = "Цього разу ми поговоримо про критичне мислення простими словами та відповімо на питання: Що таке критичне мислення? Навіщо «мислити критично»? Коли підключати критичне мислення? Як використовувати критичне мислення? Як навчитися мислити критично?",
                Date = new DateTime(2023, 5, 16),
                Time = new TimeSpan(18, 30, 00),
                Photo = null
            },
            new OnlineEvent()
            {
                Name = "ВСТУП ДО ХАІ - 2023",
                TypeId = 2,
                Description = "ШАНОВНІ АБІТУРІЄНТИ! Ми підготували для вас ♻️ онлайн-ВЕБІНАР  ВСТУП ДО ХАІ - 2023, щоб ви отримали відповіді на запитання",
                AboutEvent = "Шановний абітурієнт! Oбираєш навчальний заклад та маєш бажання отримати більше інформації про ХАІ? Маєш запитання щодо умов та особливостей навчання, освітніх можливостей і додаткових переваг? Тоді - НЕ ЗВОЛІКАЙ, РЕЄСТРУЙСЯ!",
                Date = new DateTime(2023, 05, 11),
                Time = new TimeSpan(15, 30, 00)
            },
            new OnlineEvent()
            {
                TypeId = 3,
                Name = "ЯК ВЕСТИ БІЗНЕС КОМУНІКАЦІЮ АНГЛІЙСЬКОЮ, ЯКЩО ТВІЙ РІВЕНЬ INTERMEDIATE АБО НИЖЧЕ",
                Description = "Якщо ви маєте проблеми з англійською мовою на роботі, не впевнені в собі або хочете покращити свої навички, то цей вебінар - саме для вас.",
                AboutEvent = "На вебінарі ми розглянемо: ключові елементи ефективної бізнес комунікації; реальні приклади з помилками, які руйнують ефективну бізнес комунікацію; найважливіші англійські слова та вирази, необхідні для успішної бізнес комунікації; як спілкуватися англійською, якщо є страх або невпевненність в собі.",
                Date = new DateTime(2023, 04, 27),
                Time = new TimeSpan(17, 30, 00)
            },
            new OnlineEvent()
            {
                TypeId = 2,
                Name = "Як стати бізнес-аналітиком в ІТ: початок успішної кар'єри",
                Description = "Не пропусти можливість дізнатись більше про одну з найцікавіших та найбільш перспективних професій в ІТ. Долучайся до нашої онлайн-спільноти та розширюй свої можливості разом з нами!",
                AboutEvent = "Про що поговоримо: Хто такі БА? Які плюси та мінуси професії? Які риси характеру потрібні для того, щоб бути успішним БА? Як розпочати свій шлях? Як покращити шанси при пошуку роботи? Що робити тим, хто опинився за кордоном?",
                Date = new DateTime(2023, 03, 30),
                Time = new TimeSpan(19, 00, 00)
            }
        };
    }

    private static IEnumerable<Speaker> GetSpeakers()
    {
        return new List<Speaker>()
        {
            new Speaker()
            {
                FirstName = "Анна",
                LastName = "Дяченко",
                Position = "Human Resourses Manager",
                ShortDescription = "досвідчений Human Resourses Manager та фахівець своєї справи"
            },
            new Speaker()
            {
                FirstName = "Юлія",
                LastName = "Колчаг",
                Position = "Business Development Manager English For IT",
                ShortDescription = "Вона ділитиметься своїм досвідом та надасть практичні поради, які допоможуть вам підвищити рівень своєї англійської мови та стати більш ефективним у бізнес комунікації."
            },
            new Speaker()
            {
                FirstName = "Микита",
                LastName = "Самко",
                Position = "Senior Software Engineer",
                ShortDescription = "Наш магістр та вже давно досвідчений IT-фахівець"
            },
            new Speaker()
            {
                FirstName = "Вікторія",
                LastName = "Кравченко",
                Position = "Бізнес-аналітик",
                ShortDescription = "Досвідчений бізнес-аналітик та фахівець у своєму ділі"
            },
            new Speaker()
            {
                FirstName = "Кафедра",
                LastName = "603",
                ShortDescription = "Кафедра інженерії програмного забезпечення"
            }
        };
    }

    private static IEnumerable<EventPlatform> GetEventPlatforms()
    {
        return new List<EventPlatform>()
        {
            new EventPlatform()
            {
                EventId = 21,
                PlatformId = 1,
                Link = "https://zoom.us"
            },
            new EventPlatform()
            {
                EventId = 22,
                PlatformId = 2,
                Link = "https://zoom.us"
            },
            new EventPlatform()
            {
                EventId = 23,
                PlatformId = 3,
                Link = "https://meet.google.com"
            },
            new EventPlatform()
            {
                EventId = 24,
                PlatformId = 4,
                Link = "https://www.microsoft.com/en/microsoft-teams/group-chat-software"
            },
        };
    }

    private static IEnumerable<MailTemplate> GetMailTemplates()
    {
        return new List<MailTemplate>()
        {
            new MailTemplate()
            {
                Name = "Default template",
                Html = @"<!DOCTYPE html>
<html lang=""en"">
                <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width"", initial-scale=1.0"">
                <meta http-equiv=""X-UA-Compatible"" content=""ie=edge"">
                <title>My Website</title>
                <link rel=""stylesheet"" href=""./style.css"">
                <link rel=""icon"" href=""./favicon.ico"" type=""image/x-icon"">
<style>
{Css}
</style>
                </head>
                <body>
                {Html}
                </body>
                </html>",
                Css = null,
                DefaultTemplate = true
            },
            new MailTemplate()
            {
                Name = "Welcome Platform",
                Html = @"
                    <h1>Ласкаво просимо до нашої платформи</h1>
                    <p>Шановний(а) {UserName}, дякуємо за реєстрацію на нашій платформі.</p>
                    <p>Для початку використання платформи перейдіть за посиланням нижче:</p>
                    <a href=""{ActivationLink}"">Активувати обліковий запис</a>",
                Css = null,
                WelcomeTemplate = true
            },
            new MailTemplate()
            {
                Name = "Invite event",
                Html = @"
    <div class=""hidden-text"">We're thrilled to have you here! Get ready to dive into your new account.</div>
    <div class=""container"">
        <div class=""header"">
            <div class=""header-logo"">
                <h1>Welcome @Model?.Name!</h1>
                <img src=""https://img.icons8.com/clouds/100/000000/handshake.png"" width=""125"" height=""120"" style=""display: block; border: 0;"" alt=""Handshake Icon"" />
            </div>
        </div>
        <div class=""content"">
            <p>We're excited to have you get started at the MailKit Demo Platform. First, you need to confirm your account. Just press the button below.</p>
        </div>
        <div class=""button-container"">
            <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                <tr>
                    <td align=""center"">
                        <table border=""0"" cellspacing=""0"" cellpadding=""0"">
                            <tr>
                                <td align=""center"">
                                    <a href=""#"" target=""_blank"" class=""button"">Confirm Account</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class=""link-container"">
            <div class=""content"">
                <p>If that doesn't work, copy and paste the following link in your browser:</p>
                <p><a href=""#"" target=""_blank"" class=""link"">Some URL to activate the account</a></p>
                <p>If you have any questions, just reply to this email—we're always happy to help out.</p>
            </div>
        </div>
        <div class=""content"">
            <p>Cheers,<br>BBB Team</p>
        </div>
    </div>
    <div class=""footer"">
        <div class=""container"">
            <div class=""footer-content"">
                <h2>Need more help?</h2>
                <p><a href=""#"" target=""_blank"">We're here to help you out</a></p>
            </div>
        </div>
    </div>",
                Css = @"body {
            background-color: #f4f4f4;
            margin: 0 !important;
            padding: 0 !important;
        }

        .hidden-text {
            display: none;
            font-size: 1px;
            color: #fefefe;
            line-height: 1px;
            font-family: 'Lato', Helvetica, Arial, sans-serif;
            max-height: 0px;
            max-width: 0px;
            opacity: 0;
            overflow: hidden;
        }

        .container {
            max-width: 600px;
            margin: 0 auto;
        }

        .header {
            background-color: #FFA73B;
            padding: 40px 10px;
        }

        .header-logo {
            padding: 40px 20px 20px 20px;
            border-radius: 4px 4px 0 0;
            color: #111111;
            font-family: 'Lato', Helvetica, Arial, sans-serif;
            font-size: 48px;
            font-weight: 400;
            letter-spacing: 4px;
            line-height: 48px;
            text-align: center;
        }

        .header-logo h1 {
            font-size: 48px;
            font-weight: 400;
            margin: 2;
        }

        .content {
            padding: 20px 30px;
            background-color: #ffffff;
            color: #666666;
            font-family: 'Lato', Helvetica, Arial, sans-serif;
            font-size: 18px;
            font-weight: 400;
            line-height: 25px;
        }

        .content p {
            margin: 0;
        }

        .button-container {
            padding: 20px 30px 60px 30px;
        }

        .button {
            border-radius: 3px;
            background-color: #FFA73B;
            display: inline-block;
            font-size: 20px;
            font-family: Helvetica, Arial, sans-serif;
            color: #ffffff;
            text-decoration: none;
            padding: 15px 25px;
            border: 1px solid #FFA73B;
        }

        .button a {
            color: #ffffff;
            text-decoration: none;
        }

        .link-container {
            padding: 0px 30px;
        }

        .link {
            color: #FFA73B;
        }

        .footer {
            padding: 30px 10px 0px 10px;
            background-color: #f4f4f4;
        }

        .footer-content {
            padding: 30px;
            background-color: #FFECD1;
            border-radius: 4px;
            color: #666666;
            font-family: 'Lato', Helvetica, Arial, sans-serif;
            font-size: 18px;
            font-weight: 400;
            line-height: 25px;
        }

        .footer-content h2 {
            font-size: 20px;
            font-weight: 400;
            color: #111111;
            margin: 0;
        }

        .footer-content a {
            color: #FFA73B;
        }"
            }
        };
    }
}
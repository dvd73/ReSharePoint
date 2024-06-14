using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.Common
{
    internal class Consts
    {
        public const string USER_PROFILE_WATCHER_JOB_NAME = "User Profile Watcher";
        public const string LOGGER_LIST_NAME = "Application Log";
        public const string SCRIPTMESSAGE = @"$('#id').text;";

        public const string SiteResourceFile = "berggren_site";
        public const string ConfigResourceFile = "berggren_config";
        public const string WebpartResourceFile = "berggren_webparts";
        public const string MainNavigationStartUrl = "MainNaviStartUrl";

        public const string ORDER_NEED_TO_BE_RECALCULATED = "Order need to be recalculated";
        public const string WARNING1 = "Нельзя изменить корзину заявки в состоянии '{0}'";
        public const string WARNING2 = "Нельзя удалить товар, т.к. он используется в {0} заявках";
        public const string WARNING3 = "Нельзя удалить департамент, т.к. он используется в {0} заявках";
        public const string WARNING4 = "Нельзя удалить заявку в состоянии {0}";
        public const string WARNING5 = "Недостаточно товара({0}) для заказа {1} позиций";
        public const string ITEMS_ENOUGHT = "достаточное количество";
        public const string ITEMS_NOTMORE = "заканчиваются";
        public const string ITEMS_NOTENOUGHT = "недостаточно для заказа";

        public const string SS_ORDER_NOTIFICATION_SERVICE = "Использовать уведомления о заявках";
        public const string SS_ARTICLE_NOTIFICATION_SERVICE = "Использовать уведомления о балансе товаров";
        public const string SS_ORDER_COUNT = "Счетчик заявок";
        public const string SS_GS_DEPARTMENT_ADMINISTRATOR = "Имя группы руководителей отделов/департаментов";
        public const string SS_GS_CFO = "Имя группы CFO";
        public const string SS_GS_RECEPTION = "Имя группы Reception";
        public const string SS_GS_SYSTEM_ADMINISTRATOR = "Имя группы системных администраторов";
        public const string SS_GS_ADMIN_MANAGER = "Имя группы административных менеджеров";
        public const string SS_ORDER_PAGE_SIZE = "Количество товаров на одной странице заявки";
        public const string SS_Approval_THESHOLD_1 = "Порог согласования №1";
        public const string SS_Approval_THESHOLD_2 = "Порог согласования №2";
        public const string SS_Approval_THESHOLD_3 = "Порог согласования №3";
        public const string SS_Approval_THESHOLD_4 = "Порог согласования №4";
        public const string SS_STORE_EXCEED_LIMIT_MSG = "Сообщение о превышении лимита товара при выборе в заявке";
        public const string SS_ORDER_OVERDUE_DAYS = "Кол-во дней просрочки выдачи заявок";
        public const string SS_REMAINS_WITH_PICTURES = "Представление остатка товаров с изображениями";

        public const string SS_DEFAULT_GS_DEPARTMENT_ADMINISTRATOR = "Руководители департаментов";
        public const string SS_DEFAULT_GS_CFO = "CFO";
        public const string SS_DEFAULT_GS_RECEPTION = " Reception";
        public const string SS_DEFAULT_GS_SYSTEM_ADMINISTRATOR = "Системные администраторы";
        public const string SS_DEFAULT_GS_ADMIN_MANAGER = "Административные менеджеры";
        public const string SS_DEFAULT_STORE_EXCEED_LIMIT_MSG = "Указанное количество превышает доступное на складе";
        public const int SS_DEFAULT_ORDER_OVERDUE_DAYS = 2;
        public const string SS_DEFAULT_REMAINS_WITH_PICTURES = "~site/Lists/StoreItems/RemainsWithPicture.aspx";

        public const string TOP_20_CATEGORY = "Часто заказывают";
        public const string SEARCH_RESULT_CATEGORY = "Результаты поиска";
        public const string NO_IMAGE_PICTURE_URL = "~site/Lists/SiteImages/noimage.png";
        public const string NOT_STD_FIRST_CATEGORY = "Все товары";

        public static SPContentTypeId CT_STOREITEM = new SPContentTypeId("0x0100ddcf54dfa5494411bacd914e6079cc5900539927e950d44a0295d1c6b88a9b8594");
        public static SPContentTypeId CT_STOREITEMNOTSTD = new SPContentTypeId("0x0100ddcf54dfa5494411bacd914e6079cc590041125bb5d33947c5bcef6d1615ac3843");

        public class ListUrl
        {
            public const string USERPROFILECHANGES = "~site/Lists/UserProfileChanges";
            public const string NOTIFICATIONREASONS = "~sitecollection/Lists/NotificationReasons";
            public const string DEPARTMENTS = "~site/Lists/Departments";
            public const string ORDERS = "~site/Lists/Orders";
            public const string ORDERITEMS = "~site/Lists/OrderItems";
            public const string STOREITEMS = "~site/Lists/StoreItems";
            public const string CATEGORIES = "~site/Lists/Categories";
            public const string CATEGORIES_NOTSTD = "~site/Lists/CategoriesNotStd";
            public const string STATUSES = "~site/Lists/Statuses";
            public const string ORDERLOG = "~site/Lists/OrderLog";
        }

        public class UserProfileChangesListFields
        {
            public const string Title = "Title";
            public const string UPPublicUrl = "UPPublicUrl";
            public const string UPCAUrl = "UPCAUrl";
            public const string ModifiedUser = "ModifiedUser";
            public const string PropertyName = "PropertyName";
            public const string PropertyValue = "PropertyValue";
            public const string Notificate = "Notificate";
            public const string UPChangeDate = "UPChangeDate";
            public const string ProfileID = "ProfileID";
        }

        public class DepartmentsListFields
        {
            public const string ShortName = "SPCAFContrib_Demo_DepartmentShortName";
            public const string CostCenter = "SPCAFContrib_Demo_DepartmentCostCenter";
            public const string Manager = "SPCAFContrib_Demo_DepartmentChiefManager";
            public const string BU = "SPCAFContrib_Demo_DepartmentBU";
        }

        public class OrdersListFields
        {
            public const string OrderType = "SPCAFContrib_Demo_OrderType";
            public const string Department = "SPCAFContrib_Demo_OrderDepartment";
            public const string BU = "SPCAFContrib_Demo_DepartmentBU";
            public const string CostCenter = "SPCAFContrib_Demo_DepartmentCostCenter";
            public const string ChiefManager = "SPCAFContrib_Demo_DepartmentChiefManager";
            public const string Status = "SPCAFContrib_Demo_OrderStatus";
            public const string Price = "SPCAFContrib_Demo_OrderPrice";
            public const string Comments = "V3Comments";
            public const string ClosedDate = "SPCAFContrib_Demo_OrderClosed";
            public const string CanceledDate = "SPCAFContrib_Demo_OrderCanceled";
            public const string RejectedDate = "SPCAFContrib_Demo_OrderRejected";
        }

        public class OrderItemsListFields
        {
            /// <summary>
            /// Номер заказа. Ссылка на заказ
            /// </summary>
            public const string OrderNumber = "SPCAFContrib_Demo_OrderItemOrder";
            /// <summary>
            /// Тип заказа: стандартный или нет
            /// </summary>
            public const string OrderType = "SPCAFContrib_Demo_OrderType";
            /// <summary>
            /// Статус заказа
            /// </summary>
            public const string OrderStatus = "SPCAFContrib_Demo_OrderStatus";
            public const string Article = "SPCAFContrib_Demo_OrderItemArticle";
            public const string Title = "SPCAFContrib_Demo_ItemTitle";
            public const string Category = "SPCAFContrib_Demo_ItemCategory";
            public const string MeasureUnit = "SPCAFContrib_Demo_ItemMeasureUnit";
            public const string Price = "SPCAFContrib_Demo_ItemPrice";
            /// <summary>
            /// Кол-во товара в заказе
            /// </summary>
            public const string Number = "SPCAFContrib_Demo_OrderItemAmount";
            /// <summary>
            /// Стоимость заказанного товара
            /// </summary>
            public const string Sum = "SPCAFContrib_Demo_OrderItemSum";
        }

        public class StoreItemsListFields
        {
            /// <summary>
            /// Наименование
            /// </summary>
            public const string Title = "SPCAFContrib_Demo_ItemTitle";
            /// <summary>
            /// Категория (lookup)
            /// </summary>
            public const string Category = "SPCAFContrib_Demo_ItemCategory";
            /// <summary>
            /// Категория для нестандартных товаров (текст)
            /// </summary>
            public const string CategoryNotStd = "SPCAFContrib_Demo_ItemCategoryNotStd";
            /// <summary>
            /// Изображение товара (url field)
            /// </summary>
            public const string Picture = "SPCAFContrib_Demo_ItemPicture";
            /// <summary>
            /// Единицы измерения (choice)
            /// </summary>
            public const string MeasureUnit = "SPCAFContrib_Demo_ItemMeasureUnit";
            /// <summary>
            /// Единицы измерения для нестандартных товаров (текст)
            /// </summary>
            public const string MeasureUnitNotStd = "SPCAFContrib_Demo_ItemMeasureUnitNotStd";
            /// <summary>
            /// Стоимость одной единицы соответствующей канцелярской принадлежности. Денежный формат.
            /// </summary>
            public const string Price = "SPCAFContrib_Demo_ItemPrice";
            /// <summary>
            /// В наличии. Объем, присутствующий физически на складе. Целое число.
            /// </summary>
            public const string Number = "SPCAFContrib_Demo_ItemAvailability";
            /// <summary>
            /// Индикатор. - зеленый, канцелярских принадлежностей достаточное количество: «В наличии» > «Объем предупреждения»;- желтый, канцелярские принадлежности заканчиваются: «Неснижаемый остаток» &lt; «В наличии» ≤ «Объем предупреждения»;- красный, количество канцелярских принадлежностей недостаточно для заказа: «В наличии» ≤ «Неснижаемый остаток».
            /// </summary>
            public const string Indicator = "SPCAFContrib_Demo_ItemStatusIndicator";
            /// <summary>
            /// Заказано раз. Счетчик количества раз заказа текущей канцелярской принадлежности. Служит для вычисления наиболее часто заказываемых канцелярских принадлежностей. Целое число.
            /// </summary>
            public const string OrderedNumber = "SPCAFContrib_Demo_ItemOrderCount";
            /// <summary>
            /// Приход. Объем канцелярских принадлежностей, которые поступили на склад от поставщика канцелярских принадлежностей. Целое число.
            /// </summary>
            public const string Income = "SPCAFContrib_Demo_ItemIncome";
            /// <summary>
            /// Объем предупреждения. Объем соответствующих канцелярских принадлежностей, при котором ответственное за управление складом лицо должно получить оповещение электронной почты о том, что на складе подходят к концу текущие канцелярские принадлежности.Условие для оповещения: «В наличии» ≤ «Объем предупреждения». Целое число.
            /// </summary>
            public const string WarningThreshold = "SPCAFContrib_Demo_ItemThreshold";
            /// <summary>
            /// Неснижаемый остаток. Объем соответствующих канцелярских принадлежностей, при котором ответственное за управление складом лицо должно получить оповещение электронной почты о том, что на складе осталось критическое количество текущих канцелярских принадлежностей. Условие: «В наличии» ≤ «Критический объем». Целое число.
            /// </summary>
            public const string ReserveBalance = "SPCAFContrib_Demo_ItemReserveBalance";
            /// <summary>
            /// Зарезервировано. Объем соответствующих канцелярских принадлежностей зарезервированных в соответствии с заявками, находящимися в работе. Целое число.
            /// </summary>
            public const string Reserved = "SPCAFContrib_Demo_ItemReserved";
            /// <summary>
            /// Видимость товара для заказа
            /// </summary>
            public const string NotVisible = "SPCAFContrib_Demo_ItemNotVisible";
        }

        public class OrderLogListFields
        {
            public const string OrderNumber = "SPCAFContrib_Demo_OrderItemOrder";
            public const string EventDate = "SPCAFContrib_Demo_OrderLogDate";
            public const string PrevStatus = "SPCAFContrib_Demo_OrderPrevStatus";
            public const string Status = "SPCAFContrib_Demo_OrderStatus";
            public const string Person = "SPCAFContrib_Demo_OrderLogPerson";
            public const string Description = "V3Comments";
        }

        public class UserNotificationTemplatesListFields
        {
            public const String Id = "ID";
            public const String Title = "Title";
            public const String AssignedTo = "AssignedTo";
            public const String NotificationReason = "NotificationReason";
            public const String NotificationSubject = "NotificationSubject";
            public const String Body = "Body";
        }

        public class NotificationReasonListFields
        {
            public const String Id = "ID";
            public const String Title = "Title";
            public const String DefaultNotificationTemplate = "DefaultNotificationTemplate";
        }

        public class StatusesListFields
        {
            public const string Name = "Title";
            public const string WorkflowStep = "V3Comments";
        }

        public class NavigationListFields
        {
            public const string PageRef = "URL";
            public const string OrderSequence = "ItemOrder";
            public const string MenuType = "NavigationGroup";
            public const string UserRole = "SPCAFContrib_Demo_UserRole";
        }

        public static string[] ARTICLE_SEARCH_FIELDS = new string[] { "Title", Consts.StoreItemsListFields.Title };
    }
}

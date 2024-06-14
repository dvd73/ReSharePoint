using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCAFContrib.Demo.Common
{
    public class Enums
    {
        public enum OrderStatus
        {
            Draft = 1,                  // Черновик
            AdministratorApproval = 2,  // На согласовании у администратора 
            DepartmentManagerApproval = 3,// На согласовании у руководителя отдела/департамента	
            CFOApproval = 4,            // На согласовании у CFO / Waiting CFO approval
            Rejected = 5,               // Отклонена
            Preparation = 6,            // Оформление заказа
            WaitingForDelivery = 7,     // Ожидает поставки
            Assembling = 8,             // Подбор заказа
            Ready = 9,                  // Заказ готов к выдаче
            Closed = 10,                // Закрыта
            Canceled = 11               // Отменена
        }

        public enum CatalogType
        {
            Standard = 1,
            NonStandard = 2
        }

        public enum OrderItemsMode
        {
            ReadOnly = 1,
            ChangeNumber = 2,
            GetNewItems = 3
        }

        public enum WorkflowApprovalRole
        {
            Initiator,
            Administrator,
            DepartmentManager,
            CFO
        }

        public enum SecurityGroup
        {
            DepartmentManager = 0,  // Руководитель департамента
            CFO = 1,                // CFO
            Reception = 2,          // Reception
            SystemAdministrator = 3,// Системный администратор
            AdministrativeManager = 4      // Административный менеджер
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CamlexNET;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using MOSS.Common.Code;
using MOSS.Common.Controls;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;

namespace SPCAFContrib.Demo.Common.Iterators
{
	public class UserNotificationTemplateListFieldIterator : BaseListFieldIterator
	{
		#region Fields
		protected static List<FieldDisplayRuleItem> _staticRules = new List<FieldDisplayRuleItem>(); 
		#endregion

		#region Ctor
		static UserNotificationTemplateListFieldIterator()
		{
			
		} 
		#endregion

		#region Control lifecycle
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);			

			CreateDynamicExceptions();
		}

		#endregion

		#region Methods
		protected void CreateDynamicExceptions()
		{
			_dynamicRules.AddRange(_staticRules);

			// проверим, является ли этот шаблон дефалтовым для какой либо из причин
			Web.TryUsingList(Consts.ListUrl.NOTIFICATIONREASONS, (list) =>
			{
				string myId = StringHelper.CheckForNull(ListItem[SPBuiltInFieldId.ID]);

				if (!String.IsNullOrEmpty(myId))
				{
					SPQuery query = new SPQuery();
					query.Query = Camlex.Query().Where(x => x[Consts.NotificationReasonListFields.DefaultNotificationTemplate] == (DataTypes.LookupId)myId).ToString();

					bool IsDefault = list.GetItems(query).Count > 0;

					if (IsDefault)
					{
						_dynamicRules.Add(
							 new FieldDisplayRuleItem
							 {
								 FieldNames = new List<String>() { Consts.UserNotificationTemplatesListFields.AssignedTo},
								 ControlModes = new List<SPControlMode>() { SPControlMode.Display, SPControlMode.Edit },
								 Rule = FieldDisplayRule.Hidden
							 });
					}
				}
			});

		}

		
		#endregion

	}
}

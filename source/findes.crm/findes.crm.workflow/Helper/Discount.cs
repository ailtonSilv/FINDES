using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Crm.Sdk.Messages;

namespace findes.crm.workflow.Helper
{
	class Discount
	{
		public static DataCollection<Entity> GetQuoteProducts(Guid quoteId, IOrganizationService service)
		{
			var query = new QueryExpression("quotedetail");
			query.ColumnSet = new ColumnSet("findes_margemlucratividade", "findes_valortotaldescontoalcada", "findes_valorsomadescontoalcada", "productid", "extendedamount", "manualdiscountamount", "quantity");
			query.Criteria.AddCondition("quoteid", ConditionOperator.Equal, quoteId);

			return service.RetrieveMultiple(query).Entities;
		}

		public static Entity GetDiscountRule(DataCollection<Entity> teams, IOrganizationService service)
		{
			var query = new QueryExpression("findes_regradedesconto");
			query.ColumnSet = new ColumnSet("findes_porcentagemdesconto", "findes_equipeid", "findes_descontoassociadoid");
			var filter = query.Criteria.AddFilter(LogicalOperator.Or);
			foreach (var team in teams)
			{
				filter.AddCondition("findes_equipeid", ConditionOperator.Equal, team.Id);
			}
			var linkEntity = query.AddLink("findes_desconto", "findes_descontoassociadoid", "findes_descontoid");
			linkEntity.LinkCriteria.AddCondition("findes_ativardesconto", ConditionOperator.Equal, true);
			linkEntity.LinkCriteria.AddCondition("statecode", ConditionOperator.Equal, 0);
			linkEntity.LinkCriteria.AddCondition("findes_iniciovigencia", ConditionOperator.LessEqual, DateTime.UtcNow);
			linkEntity.LinkCriteria.AddCondition("findes_terminovigencia", ConditionOperator.GreaterEqual, DateTime.UtcNow);
			linkEntity.LinkCriteria.AddCondition("findes_tipodesconto", ConditionOperator.Equal, 482870001);
			
			query.TopCount = 1;
			query.PageInfo.ReturnTotalRecordCount = false;
			var ret = service.RetrieveMultiple(query);
			if (ret.Entities.Count == 0)
			{
				throw new InvalidPluginExecutionException("Não foi possivel encontrar o desconto vinculado ao time do usuário do contexto.");
			}
			return ret.Entities.FirstOrDefault();
		}

		public static DataCollection<Entity> GetUserTeams(Guid userId, IOrganizationService service)
		{
			var query = new QueryExpression("team");
			query.ColumnSet = new ColumnSet();
			var linkEntity = query.AddLink("teammembership", "teamid", "teamid");
			linkEntity.LinkCriteria.AddCondition("systemuserid", ConditionOperator.Equal, userId);

			return service.RetrieveMultiple(query).Entities;
		}
	}
}

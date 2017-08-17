using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BudgetAttempt.API
{
    [ServiceContract]
    public interface IBudget
    {

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/transactions/latest")]
        IEnumerable<Models.Transaction> GetLatestTransactionsJson(int count);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "xml/transactions/latest")]
        IEnumerable<Models.Transaction> GetLatestTransactionsXml(int count);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/transactions/lastmonth")]
        IEnumerable<Models.Transaction> GetLastMonthTransactionsJson();

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "xml/transactions/lastmonth")]
        IEnumerable<Models.Transaction> GetLastMonthTransactionsXml();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/transactions/add")]
        Models.Transaction AddTransactionJson(Models.Transaction data);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "xml/transactions/add")]
        Models.Transaction AddTransactionXml(Models.Transaction data);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/persons/all")]
        IEnumerable<Models.Person> GetAllPersonsJson();

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "xml/persons/all")]
        IEnumerable<Models.Person> GetAllPersonsXml();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/persons/merge")]
        Models.Person MergePersonJson(Models.Person data);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "xml/persons/merge")]
        Models.Person MergePersonXml(Models.Person data);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/categories/create")]
        Models.TransactionCategory CreateCategoryJson(Models.TransactionCategory data);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "xml/categories/create")]
        Models.TransactionCategory CreateCategoryXml(Models.TransactionCategory data);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "json/categories/all")]
        IEnumerable<Models.TransactionCategory> GetAllCategoriesJson();

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, UriTemplate = "xml/categories/all")]
        IEnumerable<Models.TransactionCategory> GetAllCategoriesXml();

        [WebInvoke(Method = "OPTIONS", UriTemplate = "")]
        void GetOptions();
    }


}

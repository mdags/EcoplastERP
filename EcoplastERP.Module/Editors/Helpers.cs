using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DevExpress.Xpo;
using DevExpress.Utils;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl;
using EcoplastERP.Module.BusinessObjects.SystemObjects;

namespace EcoplastERP.Module
{
    public class Helpers
    {
        public static Control axctrl;
        public const string EmailRegularExpression = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";
        public static bool CheckIdentity(string tcKimlikNo)
        {
            bool returnvalue = false;
            if (tcKimlikNo.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                TcNo = Int64.Parse(tcKimlikNo);

                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;

                C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
            }
            return returnvalue;
        }
        public static string FillZero(string val, int length)
        {
            string rval = "", zero = "";
            for (int i = 0; i < length - val.Length; i++)
            {
                zero += "0";
            }
            rval = zero + val;

            return rval;
        }
        public static string GetSequenceSerie(string serie, string sequentNumber, int length, DateTime systemDate)
        {
            return serie + GetDateSeries(sequentNumber, length, systemDate);
        }
        public static string GetDateSeries(string sequentnum, int length, DateTime systemDate)
        {
            string year = systemDate.Year.ToString().Substring(2, 2);
            string monthCode = "";
            switch (systemDate.Month)
            {
                case 1:
                    monthCode = systemDate.Month.ToString();
                    break;
                case 2:
                    goto case 1;
                case 3:
                    goto case 1;
                case 4:
                    goto case 1;
                case 5:
                    goto case 1;
                case 6:
                    goto case 1;
                case 7:
                    goto case 1;
                case 8:
                    goto case 1;
                case 9:
                    goto case 1;
                case 10:
                    monthCode = "A";
                    break;
                case 11:
                    monthCode = "B";
                    break;
                case 12:
                    monthCode = "C";
                    break;
            }
            string day = systemDate.Day.ToString();
            if (systemDate.Day.ToString().Length == 1) day = "0" + systemDate.Day.ToString();
            return year + monthCode + day + FillZero(sequentnum, length);
        }
        public static DateTime GetSystemDate(Session session)
        {
            return Convert.ToDateTime(session.ExecuteScalar("select GETDATE()"));
        }
        public static string GetDocumentNumber(Session session, string type)
        {
            SequenceSerie serie = session.FindObject<SequenceSerie>(CriteriaOperator.Parse("TypeName = ?", type));
            DateTime systemDate = Convert.ToDateTime(session.ExecuteScalar("select GETDATE()"));
            if (serie != null)
            {
                OidGenerator oidGenerator = session.FindObject<OidGenerator>(CriteriaOperator.Parse("Type = ?", type));
                if (oidGenerator != null)
                {
                    if (serie.SerieChangePeriod == SerieChangePeriod.Daily)
                    {
                        DateTime generatedDate = (DateTime)session.ExecuteScalar(string.Format("select GeneratedDate from IDGeneratorTable where Type = '{0}'", type));
                        if (generatedDate.Date != systemDate.Date)
                            session.ExecuteNonQuery("update IDGeneratorTable set Oid = 0, GeneratedDate = @systemDate where Type = @type", new string[] { "@systemDate", "@type" }, new object[] { systemDate, type });
                    }
                    else if (serie.SerieChangePeriod == SerieChangePeriod.Monthly)
                    {
                        DateTime generatedDate = (DateTime)session.ExecuteScalar(string.Format("select GeneratedDate from IDGeneratorTable where Type = '{0}'", type));
                        if (generatedDate.Month != systemDate.Month)
                            session.ExecuteNonQuery("update IDGeneratorTable set Oid = 0, GeneratedDate = @systemDate where Type = @type", new string[] { "@systemDate", "@type" }, new object[] { systemDate, type });
                    }
                    else if (serie.SerieChangePeriod == SerieChangePeriod.Yearly)
                    {
                        DateTime generatedDate = (DateTime)session.ExecuteScalar(string.Format("select GeneratedDate from IDGeneratorTable where Type = '{0}'", type));
                        if (generatedDate.Year != systemDate.Year) session.ExecuteNonQuery("update IDGeneratorTable set Oid = 0, GeneratedDate = @systemDate where Type = @type", new string[] { "@systemDate", "@type" }, new object[] { systemDate, type });
                    }
                }
                return GetSequenceSerie(serie.Serie, DistributedIdGeneratorHelper.Generate(session.DataLayer, type, string.Empty).ToString(), serie.Long, systemDate);
            }
            else
            {
                OidGenerator oidGenerator = session.FindObject<OidGenerator>(CriteriaOperator.Parse("Type = ?", type));
                if (oidGenerator != null)
                {
                    DateTime generatedDate = (DateTime)session.ExecuteScalar(string.Format("select GeneratedDate from IDGeneratorTable where Type = '{0}'", type));
                    if (generatedDate.Date != systemDate.Date) session.ExecuteNonQuery("update IDGeneratorTable set Oid = 0, GeneratedDate = @systemDate where Type = @type", new string[] { "@systemDate", "@type" }, new object[] { systemDate, type });
                }
                return GetSequenceSerie("A", DistributedIdGeneratorHelper.Generate(session.DataLayer, type, string.Empty).ToString(), 9, systemDate);
            }
        }
        public static decimal GetExchangeRate(Session session, DateTime rateDate, Currency currency, CurrencyType currencyType)
        {
            if (currency.Code == "TL") return 0;
            decimal rate = 0;
            var exchangeRate = session.FindObject<ExchangeRate>(CriteriaOperator.Parse("RateDate = ? and Currency = ? and CurrencyType = ?", Convert.ToDateTime(rateDate.ToShortDateString()), currency, currencyType));
            if (exchangeRate != null)
            {
                rate = exchangeRate.Rate;
            }
            else
            {
                if (IsConnectedToInternet())
                {
                    string xmlAddress = string.Empty;
                    string dayname = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)rateDate.DayOfWeek];
                    if (dayname == "Cumartesi")
                    {
                        rateDate = rateDate.AddDays(-1);
                    }
                    else if (dayname == "Pazar")
                    {
                        rateDate = rateDate.AddDays(-2);
                    }
                    else if (rateDate.Date == DateTime.Now.Date)
                    {
                        if (rateDate.Day == DateTime.Now.Day)
                        {
                            if (rateDate.Hour < 16) rateDate.AddDays(-1);
                        }
                        else if (rateDate.Day > DateTime.Now.Day)
                        {
                            if (DateTime.Now.Hour < 16)
                            {
                                rateDate = DateTime.Now.AddDays(-1);
                            }
                            else rateDate = DateTime.Now;
                        }
                    }

                    if (rateDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                        xmlAddress = "http://www.tcmb.gov.tr/kurlar/today.xml";
                    else xmlAddress = String.Format("http://www.tcmb.gov.tr/kurlar/{0}{1}/{2}{1}{0}.xml", rateDate.Year, FillZero(rateDate.Month.ToString(), 2), FillZero(rateDate.Day.ToString(), 2));
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlAddress);

                    XPCollection currencies = new XPCollection(session, typeof(Currency), new BinaryOperator("IsDefault", false));
                    XPCollection currencyTypes = new XPCollection(session, typeof(CurrencyType));
                    foreach (Currency currencyItem in currencies)
                    {
                        foreach (CurrencyType currencyTypeItem in currencyTypes)
                        {
                            var newExchangeRate = new ExchangeRate(session)
                            {
                                RateDate = Convert.ToDateTime(rateDate.ToShortDateString()),
                                Currency = currencyItem,
                                CurrencyType = currencyTypeItem,
                                Rate = Convert.ToDecimal(xmlDoc.SelectSingleNode(String.Format("Tarih_Date/Currency[@Kod='{0}']/{1}", currencyItem.Code, currencyTypeItem.Code)).InnerXml) / 10000
                            };
                            newExchangeRate.Save();
                        }
                    }
                    rate = Convert.ToDecimal(xmlDoc.SelectSingleNode(String.Format("Tarih_Date/Currency[@Kod='{0}']/{1}", currency.Code, currencyType.Code)).InnerXml) / 10000;
                }
            }
            return rate;
        }
        [DllImport("wininet.dll")]
        extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static bool IsUserInRole(ISecurityUserWithRoles userWithRoles, string roleName)
        {
            Guard.ArgumentNotNull(userWithRoles, "userWithRoles");
            foreach (ISecurityRole role in userWithRoles.Roles)
            {
                if (role.Name == roleName)
                    return true;
            }
            return false;
        }
        public static bool IsUserInRole(string roleName)
        {
            return IsUserInRole(SecuritySystem.CurrentUser as ISecurityUserWithRoles, roleName);
        }
        public static bool IsUserAdministrator()
        {
            return IsUserInRole(SecuritySystem.CurrentUser as ISecurityUserWithRoles, "Administrators");
        }
    }

    public class ExcelDataBaseHelper
    {
        public static object OpenFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                MessageBox.Show("File not found");
                return null;
            }
            string connectionString = string.Empty;
            const string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
            const string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
            switch (Path.GetExtension(fileName))
            {
                case ".xls": //Excel 97-03
                    connectionString = string.Format(Excel03ConString, fileName);
                    break;
                case ".xlsx": //Excel 07
                    connectionString = string.Format(Excel07ConString, fileName);
                    break;
            }

            OleDbConnection con = new OleDbConnection(connectionString);
            con.Open();
            DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            var adapter = new OleDbDataAdapter(string.Format("select * from [{0}]", dt.Rows[0]["TABLE_NAME"].ToString()), connectionString);
            var ds = new DataSet();
            const string tableName = "excelData";
            adapter.Fill(ds, tableName);
            DataTable data = ds.Tables[tableName];
            return data;
        }
    }

    public class EnumTable
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}

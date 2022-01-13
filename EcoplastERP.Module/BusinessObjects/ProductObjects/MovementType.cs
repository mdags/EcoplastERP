using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

/*Açýlacak olan hareket kodlarý
 * P100 Ambar Giriþ
 * P101 Ambar Çýkýþ
 * P102 Satýn Alma Giriþi
 * P103 Satýn Alma Ýade
 * P112 Ambar Çýkýþ Ýptali
 * P113 Ambar Giriþ Ýptali
 * P110 Üretimden Giriþ
 * P111 Üretime Çýkýþ
 * P114 Üretimden Ýade
 * P115 Üretim Ýptali
 * P118 Fire Giriþ
 * P119 Fire Çýkýþ
 * P120 Depo Transfer Giriþi
 * P121 Depo Transfer Çýkýþý
 * P122 Kaynak Okutma Ýptali
 * P123 Kaynak Okutma Çýkýþ
 * P124 Palet Transferi Giriþ
 * P125 Palet Transferi Çýkýþ
 * P126 Sevkiyat Transferi Giriþ
 * P127 Sevkiyat Transferi Çýkýþ
 * P128 Rezervasyon Transfer Giriþi
 * P129 Rezervasyon Transfer Çýkýþý
 * P130 Sayým Eksiði Giriþi
 * P131 Sayým Fazlasý Çýkýþý
 * P140 Satýþdan Ýade
 * P141 Satýþa Çýkýþ
 */

namespace EcoplastERP.Module.BusinessObjects.ProductObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Category")]
    [DefaultProperty("Name")]
    [NavigationItem("ProductManagement")]
    public class MovementType : BaseObject
    {
        public MovementType(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        // Fields...
        private bool _Output;
        private bool _Input;
        private string _Name;
        private string _Code;

        [NonCloneable]
        [RuleUniqueValue]
        [RuleRequiredField]
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                SetPropertyValue("Code", ref _Code, value);
            }
        }

        [RuleRequiredField]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }

        public bool Input
        {
            get
            {
                return _Input;
            }
            set
            {
                SetPropertyValue("Input", ref _Input, value);
            }
        }

        public bool Output
        {
            get
            {
                return _Output;
            }
            set
            {
                SetPropertyValue("Output", ref _Output, value);
            }
        }
    }
}

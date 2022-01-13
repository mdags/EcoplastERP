using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

/*A��lacak olan hareket kodlar�
 * P100 Ambar Giri�
 * P101 Ambar ��k��
 * P102 Sat�n Alma Giri�i
 * P103 Sat�n Alma �ade
 * P112 Ambar ��k�� �ptali
 * P113 Ambar Giri� �ptali
 * P110 �retimden Giri�
 * P111 �retime ��k��
 * P114 �retimden �ade
 * P115 �retim �ptali
 * P118 Fire Giri�
 * P119 Fire ��k��
 * P120 Depo Transfer Giri�i
 * P121 Depo Transfer ��k���
 * P122 Kaynak Okutma �ptali
 * P123 Kaynak Okutma ��k��
 * P124 Palet Transferi Giri�
 * P125 Palet Transferi ��k��
 * P126 Sevkiyat Transferi Giri�
 * P127 Sevkiyat Transferi ��k��
 * P128 Rezervasyon Transfer Giri�i
 * P129 Rezervasyon Transfer ��k���
 * P130 Say�m Eksi�i Giri�i
 * P131 Say�m Fazlas� ��k���
 * P140 Sat��dan �ade
 * P141 Sat��a ��k��
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

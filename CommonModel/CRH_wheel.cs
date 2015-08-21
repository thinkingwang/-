using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Serializable]
    public partial class CRH_wheel
    {
        private static readonly NLog.Logger Nlogger = NLog.LogManager.GetLogger("Modifer");
        private byte _wheelPos;
        private byte _axlePos;

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [ReadOnly(true), DisplayName(@"����")]
        public string trainType { get; set; }

        [Key]
        [Column(Order = 1)]
        [ReadOnly(true), DisplayName(@"�����")]
        public byte axleNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [ReadOnly(true), DisplayName(@"��λ��")]
        public byte wheelNo { get; set; }

        [DisplayName(@"��λ")]
        public byte axlePos
        {
            get { return _axlePos; }
            set
            {
                Nlogger.Trace("�༭��������(CRH_wheel)��λ����ʼΪ��" + axlePos + ",�޸ĺ�Ϊ��" + value);
                _axlePos = value;
            }
        }

        [DisplayName(@"��λ")]
        public byte wheelPos
        {
            get { return _wheelPos; }
            set
            {
                Nlogger.Trace("�༭��������(CRH_wheel)��λ����ʼΪ��" + wheelPos + ",�޸ĺ�Ϊ��" + value);
                _wheelPos = value;
            }
        }
    }
}

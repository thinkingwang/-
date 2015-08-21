using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommonModel;
using CommonModel.Common;

namespace Service.Dto
{
    public class ProfileAdjustDto : Dto
    {
        private ProfileAdjust _profileDetectResult;
        /// <summary>
        /// ��ȡ����΢������������
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ProfileAdjustDto> GetAll()
        {
            var result = new List<ProfileAdjustDto>();
            var data = from v in ThrContext.Set<ProfileAdjust>()
                       select v;
            foreach (var thresholdse in data)
            {
                result.Add(new ProfileAdjustDto(thresholdse));
            }
            return result;
        }

        private ProfileAdjustDto(ProfileAdjust tt)
        {
            _profileDetectResult = tt;
        }
        [DisplayName(@"�ֺ�")]
        public wheel position
        {
            get { return (wheel)_profileDetectResult.position; }
        }
        [DisplayName(@"�־�")]
        public decimal Lj
        {
            get { return _profileDetectResult.Lj; }
            set
            {
                Nlogger.Trace("�༭��ProfileAdjust��Lj�ֶΣ���ʼΪ��" + _profileDetectResult.Lj + ",�޸ĺ�Ϊ��" + value);
                _profileDetectResult.Lj = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"��Ե���")]
        public decimal LyHd
        {
            get { return _profileDetectResult.LyHd; }
            set
            {
                Nlogger.Trace("�༭��ProfileAdjust��axleNum�ֶΣ���ʼΪ��" + _profileDetectResult.LyHd + ",�޸ĺ�Ϊ��" + value);
                _profileDetectResult.LyHd = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"��Ե�߶�")]
        public decimal LyGd
        {
            get { return _profileDetectResult.LyGd; }
            set
            {
                Nlogger.Trace("�༭��ProfileAdjust��LyGd�ֶΣ���ʼΪ��" + _profileDetectResult.LyGd + ",�޸ĺ�Ϊ��" + value);
                _profileDetectResult.LyGd = value;
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"�������")]
        public decimal LwHd
        {
            get { return _profileDetectResult.LwHd; }
            set
            {
                Nlogger.Trace("�༭��ProfileAdjust��LwHd�ֶΣ���ʼΪ��" + _profileDetectResult.LwHd + ",�޸ĺ�Ϊ��" + value);
                _profileDetectResult.LwHd = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"�������")]
        public decimal LwHd2
        {
            get { return _profileDetectResult.LwHd2; }
            set
            {
                Nlogger.Trace("�༭��ProfileAdjust��LwHd2�ֶΣ���ʼΪ��" + _profileDetectResult.LwHd2 + ",�޸ĺ�Ϊ��" + value);
                _profileDetectResult.LwHd2 = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"QRֵ")]
        public decimal QR
        {
            get { return _profileDetectResult.QR; }
            set
            {
                Nlogger.Trace("�༭��ProfileAdjust��QR�ֶΣ���ʼΪ��" + _profileDetectResult.QR + ",�޸ĺ�Ϊ��" + value);
                _profileDetectResult.QR = value;
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"�ڲ��")]
        public decimal Ncj
        {
            get { return _profileDetectResult.Ncj; }
            set
            {
                Nlogger.Trace("�༭��ProfileAdjust��Ncj�ֶΣ���ʼΪ��" + _profileDetectResult.Ncj + ",�޸ĺ�Ϊ��" + value);
                _profileDetectResult.Ncj = value;
                ThrContext.SaveChanges();
            }
        }
    }
}
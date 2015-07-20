using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommonModel;
using CommonModel.Common;

namespace Service.Dto
{
    public class CRH_wheelDto : Dto
    {
        private readonly CRH_wheel _crhWheel;

        private CRH_wheelDto(CRH_wheel crh)
        {
            _crhWheel = crh;
        }

        [ReadOnly(true), DisplayName(@"����")]
        public string trainType
        {
            get { return _crhWheel.trainType; }
        }

        [ReadOnly(true), DisplayName(@"�����")]
        public byte axleNo
        {
            get { return _crhWheel.axleNo; }
        }

        [ReadOnly(true), DisplayName(@"��λ��")]
        public wheel wheelNo
        {
            get { return (wheel) _crhWheel.wheelNo; }
        }

        [DisplayName(@"��λ")]
        public byte axlePos
        {
            get { return _crhWheel.axlePos; }
            set
            {
                Nlogger.Trace("�༭��������(CRH_wheel)��λ����ʼΪ��" + _crhWheel.axlePos + ",�޸ĺ�Ϊ��" + value);
                _crhWheel.axlePos = value;
                thrContext.SaveChanges();
            }
        }

        [DisplayName(@"��λ")]
        public byte wheelPos
        {
            get { return _crhWheel.wheelPos; }
            set
            {
                Nlogger.Trace("�༭��������(CRH_wheel)��λ����ʼΪ��" + _crhWheel.wheelPos + ",�޸ĺ�Ϊ��" + value);
                _crhWheel.wheelPos = value;
                thrContext.SaveChanges();
            }
        }

        public static IEnumerable<CRH_wheel> GetAll()
        {
            return thrContext.Set<CRH_wheel>().ToList();
        }

        public static IEnumerable<string> GetCrhWheelTypes()
        {
            //��ȡ���г���
            var trainTypes =
                (from v in thrContext.Set<CRH_wheel>() select v.trainType).Distinct();
            return trainTypes;
        }

        public static IEnumerable<CRH_wheelDto> GetCrhWheel(string trainType)
        {
            var result = new List<CRH_wheelDto>();
            var data = from v in thrContext.Set<CRH_wheel>()
                where v.trainType == trainType
                select v;
            foreach (var crh in data)
            {
                result.Add(new CRH_wheelDto(crh));
            }
            return result.OrderBy(m => m.axleNo).ThenBy(m => m.wheelPos);
        }

        public static void Delete(string trainType)
        {
            var data = from v in thrContext.Set<CRH_wheel>()
                where v.trainType == trainType
                select v;
            foreach (var crh in data)
            {
                thrContext.Set<CRH_wheel>().Remove(crh);
            }
            thrContext.SaveChanges();
        }

        public static void Delete(string trainType, byte axelNo, byte wheelNo)
        {
            var data = from v in thrContext.Set<CRH_wheel>()
                       where v.trainType == trainType&&v.axleNo ==axelNo && v.wheelNo == wheelNo
                       select v;
            foreach (var crh in data)
            {
                thrContext.Set<CRH_wheel>().Remove(crh);
            }
            thrContext.SaveChanges();
        }


        public static void Add(string trainType)
        {
            var item = thrContext.Set<CRH_wheel>().Where(m => m.trainType.Equals(trainType)).OrderByDescending(m => m.axleNo).FirstOrDefault();
            if (item != null)
            {
                var element = DeepCopy(item);
                element.axleNo = (byte) (item.axleNo + 1);
                element.wheelNo = 0;
                thrContext.Set<CRH_wheel>().Add(element);
            }
            else
            {
                thrContext.Set<CRH_wheel>().Add(new CRH_wheel()
                {
                    trainType = trainType,
                    axleNo = 0,
                    wheelNo = 0
                });
            }
            thrContext.SaveChanges();
        }
        public static void Copy(string trainType, string name)
        {
            var data = from v in thrContext.Set<CRH_wheel>()
                where v.trainType == trainType
                select v;
            foreach (var crh in data)
            {
                var holds = DeepCopy(crh);
                holds.trainType = name;
                thrContext.Set<CRH_wheel>().Add(holds);
            }
            thrContext.SaveChanges();
        }

        public static void CreateDataBase(IEnumerable<CRH_wheel> data)
        {
            foreach (var threshold in thrContext.Set<CRH_wheel>())
            {
                thrContext.Set<CRH_wheel>().Remove(threshold);
            }
            thrContext.SaveChanges();
            foreach (var thresholdse in data)
            {
                thrContext.Set<CRH_wheel>().Add(thresholdse);
            }
            thrContext.SaveChanges();
        }
    }
}
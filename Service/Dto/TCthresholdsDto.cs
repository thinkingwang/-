using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using CommonModel;
using DCModel;
using TCModel;

namespace Service.Dto
{
    public class TCthresholdsDto : Dto
    {
        private readonly TCthreshold _thresholds;
        static TCthresholdsDto()
        {
        }
        private TCthresholdsDto(threshold th)
        {
            _thresholds = th as TCthreshold;
        }

        /// <summary>
        /// �����ⲿ���ݣ���д���ݿ�����
        /// </summary>
        /// <param name="data"></param>
        public static void CreateDataBase(ArrayList data)
        {
            using (var context = new DCContext(Dto.connectionstring))
            {
                foreach (var thresholdse in (List<TCthreshold>)data[0])
                {
                    context.Set<TCthreshold>().AddOrUpdate(thresholdse);
                }
                CRH_wheelDto.CreateDataBase((List<CRH_wheel>) data[1],context);
                TrainTypeDto.CreateDataBase((List<TrainType>) data[2],context);
                if (!context.Set<ProfileAdjust>().Any())
                {
                    context.Set<ProfileAdjust>().Add(new ProfileAdjust() { position = 0 });
                    context.Set<ProfileAdjust>().Add(new ProfileAdjust() { position = 1 });
                }
                context.SaveChanges();
            }
            
            Nlogger.Trace("�����ⲿ���ݣ���д���ݿ�����");
        }

        /// <summary>
        /// ������ޱ���������
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<threshold> GetAll()
        {
            var data = from d in ThrContext.Set<TCthreshold>() select d;
            return data.ToList();
        }

        /// <summary>
        /// ������ޱ����б�
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetThresholdsTypes()
        {
            //��ȡ���г���
            var trainTypes =
                (from v in ThrContext.Set<TCthreshold>() select v.trainType).Distinct();
            return trainTypes;
        }

        /// <summary>
        /// ������ޱ���ָ�����͵���������
        /// </summary>
        /// <param name="trainType"></param>
        /// <returns></returns>
        public static IEnumerable<TCthresholdsDto> GetThresholds(string trainType)
        {
            var result = new List<TCthresholdsDto>();
            IQueryable<threshold> data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                result.Add(new TCthresholdsDto(thresholdse));
            }
            return result;
        }

        /// <summary>
        /// �����ޱ���ɾ��ָ������
        /// </summary>
        /// <param name="trainType"></param>
        public static void Delete(string trainType)
        {
            CRH_wheelDto.Delete(trainType);
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                ThrContext.Set<TCthreshold>().Remove(thresholdse);
            }
            ThrContext.SaveChanges();
            Nlogger.Trace("�����ޱ���ɾ��ָ������");
        }

        /// <summary>
        /// �½�ָ������
        /// </summary>
        /// <param name="trainType"></param>
        /// <param name="name"></param>
        public static void NewThresholds(string trainType, string name)
        {
            var item = (ThrContext.Set<TCthreshold>().FirstOrDefault(m => m.trainType.Equals(trainType))).Copy() as TCthreshold;
            if (item == null)
            {
                return;
            }
            item.name = name;
            ThrContext.Set<TCthreshold>().Add(item);
            ThrContext.SaveChanges();
            Nlogger.Trace("�½��ƶ�����");
        }

        /// <summary>
        /// ɾ��ָ������ĳ������
        /// </summary>
        /// <param name="trainType"></param>
        /// <param name="index"></param>
        public static void DeleteThresholds(string trainType, int index)
        {
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            ThrContext.Set<TCthreshold>().Remove(data.ToList().ElementAt(index));
            ThrContext.SaveChanges();
            Nlogger.Trace("ɾ��ָ������ĳ������");
        }

        /// <summary>
        /// ����ָ���������ݣ������뵽���ޱ���
        /// </summary>
        /// <param name="trainType"></param>
        /// <param name="name"></param>
        public static void Copy(string trainType, string name)
        {
            CRH_wheelDto.Copy(trainType, name);
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                var holds = thresholdse.Copy() as TCthreshold;
                if (holds == null)
                {
                    continue;
                }
                holds.trainType = name;
                ThrContext.Set<TCthreshold>().Add(holds);
            }
            ThrContext.SaveChanges();
        }

        /// <summary>
        /// ����ָ���������ݣ������뵽���ޱ���
        /// </summary>
        /// <param name="trainType"></param>
        public static void CopyPowerType(string trainType)
        {
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                var holds = thresholdse.Copy() as TCthreshold;
                if (holds == null)
                {
                    continue;
                }
                holds.powerType = 0;
                ThrContext.Set<TCthreshold>().AddOrUpdate(holds);
            }
            ThrContext.SaveChanges();
        }
        /// <summary>
        /// ����ָ���������ݣ������뵽���ޱ���
        /// </summary>
        /// <param name="trainType"></param>
        public static void DeletePowerType(string trainType)
        {
            var data = from v in ThrContext.Set<TCthreshold>()
                where v.trainType == trainType
                select v;
            foreach (var thresholdse in data)
            {
                if (thresholdse.powerType != 0)
                {
                    continue;
                }
                ThrContext.Set<TCthreshold>().Remove(thresholdse);
            }
            ThrContext.SaveChanges();
        }


        [Category("�����������"), Description("�����������"), ReadOnly(true), DisplayName(@"����")]
        public string trainType
        {
            get { return _thresholds.trainType; }
            set
            {
                Nlogger.Trace("�༭��thresholds��trainType�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.trainType + ",�޸ĺ�Ϊ��" + value);
                _thresholds.trainType = value;
                ThrContext.SaveChanges();
            }
        }



        [Category("�����������"), Description("�����������"), ReadOnly(true), DisplayName(@"��������")]
        public string ParamName
        {
            get
            {
                if (Document == null)
                {
                    return _thresholds.name;
                }
                var node = Document.SelectSingleNode(string.Format((string) "/config/add[@key='{0}']/@value", (object) _thresholds.name));
                return node == null ? _thresholds.name : node.Value;
            }
        }

        [DisplayName(@"��׼ֵ")]
        public decimal? standard
        {
            get { return _thresholds.standard; }
            set
            {
                Nlogger.Trace("�༭��thresholds��standard�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.standard + ",�޸ĺ�Ϊ��" + value);
                _thresholds.standard = value;
                if (_thresholds.up_level3 < standard)
                {
                    Up3 = "����";
                }
                if (_thresholds.up_level2 < standard)
                {
                    Up2 = "����";
                }
                if (_thresholds.up_level1 < standard)
                {
                    Up1 = "����";
                }
                if (_thresholds.low_level3 > standard)
                {
                    Low3 = "����";
                }
                if (_thresholds.low_level2 > standard)
                {
                    Low2 = "����";
                }
                if (_thresholds.low_level1 > standard)
                {
                    Low1 = "����";
                }
                ThrContext.SaveChanges();
            }
        }

        [DisplayName("��������")]
        public string Up3
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level3 - 2000))) < Convert.ToDecimal(0.01))
                {
                    return "����";
                }
                return _thresholds.up_level3.ToString();
            }
            set
            {
                Nlogger.Trace("�༭��thresholds��up_level3�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.up_level3 + ",�޸ĺ�Ϊ��" + value);
                if (value.Equals("����"))
                {
                    _thresholds.up_level3 = Convert.ToDecimal(2000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter < standard)
                    {
                        throw new Exception("����ֵ������ڱ�׼ֵ���������趨");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.up_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level1 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.up_level1))
                    {
                        _thresholds.up_level3 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("��������ֵ����С�ڵ������޶���������һ�����������趨");
                    }
                }
                ThrContext.SaveChanges();
            }
        }

        [DisplayName("���޶���")]
        public string Up2
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01))
                {
                    return "����";
                }
                return _thresholds.up_level2.ToString();
            }
            set
            {
                Nlogger.Trace("�༭��thresholds��up_level2�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.up_level2 + ",�޸ĺ�Ϊ��" + value);
                if (value.Equals("����"))
                {
                    _thresholds.up_level2 = Convert.ToDecimal(2000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter < standard)
                    {
                        throw new Exception("����ֵ������ڱ�׼ֵ���������趨");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level3 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.up_level3) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level1 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.up_level1))
                    {
                        if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01))
                        {
                            if (desc.Contains("Ԥ��"))
                            {
                                desc = desc.Remove(0, desc.IndexOf("Ԥ��") + 3);
                            }
                        }
                        else
                        {
                            if (desc.Contains("Ԥ��"))
                            {
                                desc = desc.Replace(_thresholds.up_level2.ToString(), value);
                            }
                            else
                            {
                                desc = desc.Insert(0, "����" + value + "Ԥ��,");
                            }
                        }
                        _thresholds.up_level2 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("���޶���ֵ������ڵ�������������С�ڵ�������һ�����������趨");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"����һ��")]
        public string Up1
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level1 - 2000))) < Convert.ToDecimal(0.01))
                {
                    return "����";
                }
                return _thresholds.up_level1.ToString();
            }
            set
            {
                Nlogger.Trace("�༭��thresholds��up_level1�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.up_level1 + ",�޸ĺ�Ϊ��" + value);
                if (value.Equals("����"))
                {
                    _thresholds.up_level1 = Convert.ToDecimal(2000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter < standard)
                    {
                        throw new Exception("����ֵ������ڱ�׼ֵ���������趨");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level2 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.up_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.up_level3 - 2000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.up_level3))
                    {
                        _thresholds.up_level1 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("����һ��ֵ������ڵ����������������޶������������趨");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"��������")]
        public string Low3
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level3 + 1000))) < Convert.ToDecimal(0.01))
                {
                    return "����";
                }
                return _thresholds.low_level3.ToString();
            }
            set
            {
                Nlogger.Trace("�༭��thresholds��low_level3�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.low_level3 + ",�޸ĺ�Ϊ��" + value);
                if (value.Equals("����"))
                {
                    _thresholds.low_level3 = Convert.ToDecimal(-1000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter > standard)
                    {
                        throw new Exception("����ֵ����С�ڱ�׼ֵ���������趨");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level2 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.low_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level1 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.low_level1))
                    {
                        _thresholds.low_level3 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("��������ֵ������ڵ������޶���������һ�����������趨");
                    }
                }
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"���޶���")]
        public string Low2
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level2 + 1000))) < Convert.ToDecimal(0.01))
                {
                    return "����";
                }
                return _thresholds.low_level2.ToString();
            }
            set
            {
                Nlogger.Trace("�༭��thresholds��low_level2�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.low_level2 + ",�޸ĺ�Ϊ��" + value);
                if (value.Equals("����"))
                {
                    _thresholds.low_level2 = Convert.ToDecimal(-1000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter > standard)
                    {
                        throw new Exception("����ֵ����С�ڱ�׼ֵ���������趨");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level3 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.low_level3) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level1 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter > _thresholds.low_level1))
                    {
                        _thresholds.low_level2 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("���޶���ֵ������ڵ�������һ��С�ڵ��������������������趨");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"����һ��")]
        public string Low1
        {
            get
            {
                if (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level1 + 1000))) < Convert.ToDecimal(0.01))
                {
                    return "����";
                }
                return _thresholds.low_level1.ToString();
            }
            set
            {
                Nlogger.Trace("�༭��thresholds��low_level1�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.low_level1 + ",�޸ĺ�Ϊ��" + value);
                if (value.Equals("����"))
                {
                    _thresholds.low_level1 = Convert.ToDecimal(-1000);
                }
                else
                {
                    decimal valueInter = Convert.ToDecimal(value);
                    if (valueInter > standard)
                    {
                        throw new Exception("����ֵ����С�ڱ�׼ֵ���������趨");
                    }
                    if ((Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level2 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.low_level2) &&
                        (Math.Abs(Convert.ToDecimal((object) (_thresholds.low_level3 + 1000))) < Convert.ToDecimal(0.01) ||
                         valueInter < _thresholds.low_level3))
                    {
                        _thresholds.low_level1 = Convert.ToDecimal(value);
                    }
                    else
                    {
                        throw new Exception("����һ��ֵ����С�ڵ������޶����������������������趨");
                    }
                }
                SetDesc();
                ThrContext.SaveChanges();
            }
        }
        [DisplayName(@"����")]
        public decimal? precision
        {
            get { return _thresholds.precision; }
            set
            {
                Nlogger.Trace("�༭��thresholds��precision�ֶΣ�����Ϊ��"+ trainType +",��������Ϊ��" + ParamName + "����ʼΪ��" + _thresholds.precision + ",�޸ĺ�Ϊ��" + value);
                _thresholds.precision = value;
                SetDesc();
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"����")]
        public string desc
        {
            get { return _thresholds.desc; }
            set
            {
                _thresholds.desc = value;
                ThrContext.SaveChanges();
            }
        }

        [DisplayName(@"��������")]
        public string PowerType 
        {
            get
            {
                if (_thresholds.powerType == 0)
                {
                    return "�ϳ�";
                }
                return "����";
            }
            set
            {
                ThrContext.Set<TCthreshold>().Remove(_thresholds);
                ThrContext.SaveChanges();
                if (value == "����")
                {
                    _thresholds.powerType = 1;
                }
                else
                {
                    _thresholds.powerType = 0;
                }
                ThrContext.Set<TCthreshold>().AddOrUpdate(_thresholds);
                ThrContext.SaveChanges();
            }
        }




        private void SetDesc()
        {
            if (!Up1.Equals("����") && !Up2.Equals("����") && !Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("����{0}mm��С��{1}mmԤ��������{2}mm��С��{3}mm����",
                    Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("����") && Up2.Equals("����") && !Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("С��{0}mmԤ��������{1}mm��С��{2}mm����",
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("����") && !Up2.Equals("����") && !Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("����{0}mm��С��{1}mmԤ����С��{2}mm����", Convert.ToString((object) _thresholds.up_level2),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("����") && Up2.Equals("����") && !Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("С��{0}mmԤ����С��{1}mm����", Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }

            if (!Up1.Equals("����") && !Up2.Equals("����") && Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("����{0}mmԤ��������{1}mm��С��{2}mm����",
                    Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("����") && !Up2.Equals("����") && !Low2.Equals("����") && Low1.Equals("����"))
            {
                desc = string.Format("����{0}mm��С��{1}mmԤ��������{2}mm����",
                    Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("����") && !Up2.Equals("����") && Low2.Equals("����") && Low1.Equals("����"))
            {
                desc = string.Format("����{0}mmԤ��������{1}mm����", Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", "")) ;
            }


            if (!Up1.Equals("����") && Up2.Equals("����") && Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("����{0}mm��С��{1}mm����", Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("����") && Up2.Equals("����") && !Low2.Equals("����") && Low1.Equals("����"))
            {
                desc = string.Format("С��{0}mmԤ��������{1}mm����", Convert.ToString((object) _thresholds.low_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.up_level1).Replace(".00", "")) ;
            }
            if (!Up1.Equals("����") && Up2.Equals("����") && Low2.Equals("����") && Low1.Equals("����"))
            {
                desc = string.Format("����{0}mm����", Convert.ToString((object) _thresholds.up_level1).Replace(".00", ""));
            }


            if (Up1.Equals("����") && !Up2.Equals("����") && Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("����{0}mmԤ����С��{1}mm����", Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("����") && !Up2.Equals("����") && !Low2.Equals("����") && Low1.Equals("����"))
            {
                desc = string.Format("����{0}mm��С��{1}mmԤ��", Convert.ToString((object) _thresholds.up_level2).Replace(".00", ""),
                    Convert.ToString((object) _thresholds.low_level2).Replace(".00", "")) ;
            }
            if (Up1.Equals("����") && !Up2.Equals("����") && Low2.Equals("����") && Low1.Equals("����"))
            {
                desc = string.Format("����{0}mmԤ��", Convert.ToString((object) _thresholds.up_level2).Replace(".00", "")) ;
            }


            if (Up1.Equals("����") && Up2.Equals("����") && Low2.Equals("����") && !Low1.Equals("����"))
            {
                desc = string.Format("С��{0}mm����", Convert.ToString((object) _thresholds.low_level1).Replace(".00", "")) ;
            }
            if (Up1.Equals("����") && Up2.Equals("����") && !Low2.Equals("����") && Low1.Equals("����"))
            {
                desc = string.Format("С��{0}mmԤ��", Convert.ToString((object) _thresholds.low_level2).Replace(".00", "")) ;
            }
        }
    }
}
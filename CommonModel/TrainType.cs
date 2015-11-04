using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModel
{
    [Serializable]
    [Table("TrainType")]
    public partial class TrainType
    {
        [DisplayName(@"����")]
        [Column("trainType")]
        [Required]
        [StringLength(20)]
        public string trainType { get; set; }

        [DisplayName(@"���ſ�ʼ����")]
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int trainNoFrom { get; set; }

        [DisplayName(@"���Ž�������")]
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int trainNoTo { get; set; }

        [DisplayName(@"������ʾ��ʽ")]
        [Required]
        [StringLength(20)]
        public string format { get; set; }
    }
}

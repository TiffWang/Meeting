using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BusinessTier
{
    public enum SessionName
    {
        UserId,
        LoginName,
        RoleFunctions,
        Empl,
        FunId,
        Pid,
        FinalId,
        EmplId,
        Prods,
        Nodes,
        CurRoleIds,
        BackUrl,
        TaskAssignee,
        ActivityID,

    }

    public enum ActivityType
    {
        第三方合作 = 1,
        政府合作 = 2,
        营销体验车 = 3,
        常规会议 = 4,
    }

    /// <summary>
    /// 项目规定Result结果code
    /// </summary>
    public enum ResultCode
    {
        处理成功 = 0,
        处理失败 = 1,
        未知异常 = 3,
        有必填字段值为空 = 100,
        获取流程任务信息异常 = 101,
        启动流程失败 = 102,
        完成流程任务失败 = 103,
        获取业务数据失败 = 104,
        未知任务类型 = 105,
        未登录 = 998,
        非法请求 = 999,
    }

    #region sys

    public enum SysCodeType
    {
        DeductMoneyType,
        ExceptionalType,
        LeaveType,
        LeaveStatus,
        TravelBusiness,
        sex,
        /// <summary>
        /// 最高学历
        /// </summary>
        HighestEducation,
        IsActive,
        /// <summary>
        /// 民族
        /// </summary>
        Nationality,
        //IsValid,
        State,
        GradeType,
        IsMarried,
        StatusXiongka,
        /// <summary>
        /// 单位类型(企业、事业单位等)（客户类型）
        /// </summary>
        CompanyType,
        /// <summary>
        /// 企业规模 （人数）
        /// </summary>
        CompanyScope,
        /// <summary>
        /// 行业
        /// </summary>
        Vocation,
        /// <summary>
        /// 线索来源
        /// </summary>
        ClueFlag,

        ClientType,
        BussLicence,
        /// <summary>
        /// 公司性质 (IndustryProperty)
        /// </summary>
        ComProperty,
        /// <summary>
        /// 城市
        /// </summary>
        //Areacde,
        /// <summary>
        /// 客户等级
        /// </summary>
        Comgrade,
        /// <summary>
        /// 客户意向度
        /// </summary>
        Clientwillgrade,
        /// <summary>
        /// 快速批注
        /// </summary>
        fastAnnotation,
        /// <summary>
        /// 百度资质审核未通过原因
        /// </summary>
        biaduvalidate,
        /// <summary>
        /// 资质核实地址
        /// </summary>
        zzvalidate,
        /// <summary>
        /// 百度工单网站审核
        /// </summary>
        webvalidate,
        /// <summary>
        /// 百度工单新单注册
        /// </summary>
        baiduorderreg,
        /// <summary>
        /// 百度工单北京审核
        /// </summary>
        bjvalidate,
        /// <summary>
        /// 所属城市
        /// </summary>
        city,
        /// <summary>
        /// 联系人类型
        /// </summary>
        contactType,
        /// <summary>
        /// 联系人职务
        /// </summary>
        contactPosition,
        /// <summary>
        /// 企业经营模式
        /// </summary>
        businessModel,
        /// <summary>
        /// 快递公司
        /// </summary>
        express,
        /// <summary>
        /// 付款项目（私对私，私人账号，存现，支票对公账户）
        /// </summary>
        paymentProject,
        /// <summary>
        /// 付款方式 (现金，单位帐户，个人帐户)
        /// </summary>
        paymentMethod,
        /// <summary>
        /// IDC域名注册接口
        /// </summary>
        interfaceType,
        /// <summary>
        /// 公司
        /// </summary>
        company,
        /// <summary>
        /// 寄送方式
        /// </summary>
        sendWay,

        /// <summary>
        /// 区域代理账号
        /// </summary>
        Proxyaccount,
        /// <summary>
        /// 线索问题
        /// </summary>
        clueQuestion,

    }

    /// <summary>
    /// 系统基础配置
    /// </summary>
    public enum ESysConfig
    {
        预审领发票财务审核金额 = 101,
        收据任务关闭时间 = 102,
        底单金额超出合同金额最大值 = 103,
        是否启动完善行业 = 104,
        特殊退款金额度 = 105,
        未入库线索保留期限 = 200,
        客户库容 = 201,
    }
    #endregion
}

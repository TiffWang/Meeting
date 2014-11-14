/********************************
 * 一些优化反射的方法
 * 动态属性操作的优化
 * yx 2013-07-03
 * ****************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace BusinessTier.OptimizeReflection
{

    #region 创建IGetValue或者ISetValue实例的工厂方法类


    /// <summary>
    /// 定义读属性操作的接口
    /// </summary>
    public interface IGetValue
    {
        object Get(object target);
    }
    /// <summary>
    /// 定义写属性操作的接口
    /// </summary>
    public interface ISetValue
    {
        void Set(object target, object val);
    }


    /// <summary>
    /// 创建IGetValue或者ISetValue实例的工厂方法类
    /// </summary>
    public static class GetterSetterFactory
    {
        private static readonly Hashtable s_getterDict = Hashtable.Synchronized(new Hashtable(10240));
        private static readonly Hashtable s_setterDict = Hashtable.Synchronized(new Hashtable(10240));

        internal static IGetValue GetPropertyGetterWrapper(PropertyInfo propertyInfo)
        {
            IGetValue property = (IGetValue)s_getterDict[propertyInfo];
            if (property == null)
            {
                property = CreatePropertyGetterWrapper(propertyInfo);
                s_getterDict[propertyInfo] = property;
            }
            return property;
        }

        internal static ISetValue GetPropertySetterWrapper(PropertyInfo propertyInfo)
        {
            ISetValue property = (ISetValue)s_setterDict[propertyInfo];
            if (property == null)
            {
                property = CreatePropertySetterWrapper(propertyInfo);
                s_setterDict[propertyInfo] = property;
            }
            return property;
        }

        /// <summary>
        /// 根据指定的PropertyInfo对象，返回对应的IGetValue实例
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static IGetValue CreatePropertyGetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanRead == false)
                throw new InvalidOperationException("属性不支持读操作。");

            MethodInfo mi = propertyInfo.GetGetMethod(true);

            if (mi.GetParameters().Length > 0)
                throw new NotSupportedException("不支持构造索引器属性的委托。");

            if (mi.IsStatic)
            {
                Type instanceType = typeof(StaticGetterWrapper<>).MakeGenericType(propertyInfo.PropertyType);
                return (IGetValue)Activator.CreateInstance(instanceType, propertyInfo);
            }
            else
            {
                Type instanceType = typeof(GetterWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
                return (IGetValue)Activator.CreateInstance(instanceType, propertyInfo);
            }
        }

        /// <summary>
        /// 根据指定的PropertyInfo对象，返回对应的ISetValue实例
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static ISetValue CreatePropertySetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo mi = propertyInfo.GetSetMethod(true);

            if (mi.GetParameters().Length > 1)
                throw new NotSupportedException("不支持构造索引器属性的委托。");

            if (mi.IsStatic)
            {
                Type instanceType = typeof(StaticSetterWrapper<>).MakeGenericType(propertyInfo.PropertyType);
                return (ISetValue)Activator.CreateInstance(instanceType, propertyInfo);
            }
            else
            {
                Type instanceType = typeof(SetterWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
                return (ISetValue)Activator.CreateInstance(instanceType, propertyInfo);
            }
        }
    }

    #endregion

    #region 一些扩展方法，用于访问属性，它们都可以优化反射性能

    /// <summary>
    /// 一些扩展方法，用于访问属性，它们都可以优化反射性能。
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// 快速调用PropertyInfo的GetValue方法
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object FastGetValue(this PropertyInfo propertyInfo, object obj)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            return GetterSetterFactory.GetPropertyGetterWrapper(propertyInfo).Get(obj);
        }

        /// <summary>
        /// 快速调用PropertyInfo的SetValue方法
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void FastSetValue(this PropertyInfo propertyInfo, object obj, object value)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            GetterSetterFactory.GetPropertySetterWrapper(propertyInfo).Set(obj, value);
        }
    }



    public class GetterWrapper<TTarget, TValue> : IGetValue
    {
        private Func<TTarget, TValue> _getter;

        public GetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanRead == false)
                throw new InvalidOperationException("属性不支持读操作。");

            MethodInfo m = propertyInfo.GetGetMethod(true);
            _getter = (Func<TTarget, TValue>)Delegate.CreateDelegate(typeof(Func<TTarget, TValue>), null, m);
        }

        public TValue GetValue(TTarget target)
        {
            return _getter(target);
        }
        object IGetValue.Get(object target)
        {
            return _getter((TTarget)target);
        }
    }

    public class SetterWrapper<TTarget, TValue> : ISetValue
    {
        private Action<TTarget, TValue> _setter;

        public SetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo m = propertyInfo.GetSetMethod(true);
            _setter = (Action<TTarget, TValue>)Delegate.CreateDelegate(typeof(Action<TTarget, TValue>), null, m);
        }

        public void SetValue(TTarget target, TValue val)
        {
            _setter(target, val);
        }
        void ISetValue.Set(object target, object val)
        {
            _setter((TTarget)target, (TValue)val);
        }
    }


    public class StaticGetterWrapper<TValue> : IGetValue
    {
        private Func<TValue> _getter;

        public StaticGetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanRead == false)
                throw new InvalidOperationException("属性不支持读操作。");

            MethodInfo m = propertyInfo.GetGetMethod(true);
            _getter = (Func<TValue>)Delegate.CreateDelegate(typeof(Func<TValue>), m);
        }

        public TValue GetValue()
        {
            return _getter();
        }
        object IGetValue.Get(object target)
        {
            return _getter();
        }
    }

    public class StaticSetterWrapper<TValue> : ISetValue
    {
        private Action<TValue> _setter;

        public StaticSetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo m = propertyInfo.GetSetMethod(true);
            _setter = (Action<TValue>)Delegate.CreateDelegate(typeof(Action<TValue>), m);
        }

        public void SetValue(TValue val)
        {
            _setter(val);
        }
        void ISetValue.Set(object target, object val)
        {
            _setter((TValue)val);
        }
    }

    #endregion

    #region 反射时一些委托，Emit优化反射

    public delegate object CtorDelegate();

    public delegate object MethodDelegate(object target, object[] args);

    public delegate object GetValueDelegate(object target);

    public delegate void SetValueDelegate(object target, object arg);

    public static class DynamicMethodFactory
    {
        public static CtorDelegate CreateConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");
            if (constructor.GetParameters().Length > 0)
                throw new NotSupportedException("不支持有参数的构造函数。");

            DynamicMethod dm = new DynamicMethod(
                "ctor",
                constructor.DeclaringType,
                Type.EmptyTypes,
                true);

            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);

            return (CtorDelegate)dm.CreateDelegate(typeof(CtorDelegate));
        }

        public static MethodDelegate CreateMethod(MethodInfo method)
        {
            ParameterInfo[] pi = method.GetParameters();

            DynamicMethod dm = new DynamicMethod("DynamicMethod", typeof(object),
                new Type[] { typeof(object), typeof(object[]) },
                typeof(DynamicMethodFactory), true);

            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);

            for (int index = 0; index < pi.Length; index++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, index);

                Type parameterType = pi[index].ParameterType;
                if (parameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();
                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Unbox, parameterType);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldelema, parameterType);
                    }
                }
                else
                {
                    il.Emit(OpCodes.Ldelem_Ref);

                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, parameterType);
                        il.Emit(OpCodes.Ldobj, parameterType);
                    }
                }
            }

            if ((method.IsAbstract || method.IsVirtual)
                && !method.IsFinal && !method.DeclaringType.IsSealed)
            {
                il.Emit(OpCodes.Callvirt, method);
            }
            else
            {
                il.Emit(OpCodes.Call, method);
            }

            if (method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, method.ReturnType);
            }
            il.Emit(OpCodes.Ret);

            return (MethodDelegate)dm.CreateDelegate(typeof(MethodDelegate));
        }

        public static GetValueDelegate CreatePropertyGetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanRead)
                return null;

            MethodInfo getMethod = property.GetGetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(object),
                new Type[] { typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!getMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, getMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, getMethod, null);

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);

            il.Emit(OpCodes.Ret);

            return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
        }

        public static SetValueDelegate CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.CanWrite)
                return null;

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                new Type[] { typeof(object), typeof(object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, setMethod, null);

            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }

        public static GetValueDelegate CreateFieldGetter(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            DynamicMethod dm = new DynamicMethod("FieldGetter", typeof(object),
                new Type[] { typeof(object) },
                field.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!field.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);

                EmitCastToReference(il, field.DeclaringType);  //to handle struct object

                il.Emit(OpCodes.Ldfld, field);
            }
            else
                il.Emit(OpCodes.Ldsfld, field);

            if (field.FieldType.IsValueType)
                il.Emit(OpCodes.Box, field.FieldType);

            il.Emit(OpCodes.Ret);

            return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
        }

        public static SetValueDelegate CreateFieldSetter(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            DynamicMethod dm = new DynamicMethod("FieldSetter", null,
                new Type[] { typeof(object), typeof(object) },
                field.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!field.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, field.FieldType);

            if (!field.IsStatic)
                il.Emit(OpCodes.Stfld, field);
            else
                il.Emit(OpCodes.Stsfld, field);
            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);
            else
                il.Emit(OpCodes.Castclass, type);
        }
    }

    #endregion

    #region 一些扩展方法，用于反射操作，它们都可以优化反射性能。
    /// <summary>
    /// 一些扩展方法，用于反射操作，它们都可以优化反射性能。
    /// </summary>
    public static class ReflectionExtensions
    {
        private static readonly Hashtable s_getterDict = Hashtable.Synchronized(new Hashtable(10240));
        private static readonly Hashtable s_setterDict = Hashtable.Synchronized(new Hashtable(10240));
        private static readonly Hashtable s_methodDict = Hashtable.Synchronized(new Hashtable(10240));


        public static object FastGetValue(this FieldInfo fieldInfo, object obj)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            GetValueDelegate getter = (GetValueDelegate)s_getterDict[fieldInfo];
            if (getter == null)
            {
                getter = DynamicMethodFactory.CreateFieldGetter(fieldInfo);
                s_getterDict[fieldInfo] = getter;
            }

            return getter(obj);
        }

        public static void FastSetField(this FieldInfo fieldInfo, object obj, object value)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            SetValueDelegate setter = (SetValueDelegate)s_setterDict[fieldInfo];
            if (setter == null)
            {
                setter = DynamicMethodFactory.CreateFieldSetter(fieldInfo);
                s_setterDict[fieldInfo] = setter;
            }

            setter(obj, value);
        }


        public static object FastNew(this Type instanceType)
        {
            if (instanceType == null)
                throw new ArgumentNullException("instanceType");

            CtorDelegate ctor = (CtorDelegate)s_methodDict[instanceType];
            if (ctor == null)
            {
                ConstructorInfo ctorInfo = instanceType.GetConstructor(Type.EmptyTypes);
                ctor = DynamicMethodFactory.CreateConstructor(ctorInfo);
                s_methodDict[instanceType] = ctor;
            }

            return ctor();
        }




        public static object FastGetValue2(this PropertyInfo propertyInfo, object obj)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            GetValueDelegate getter = (GetValueDelegate)s_getterDict[propertyInfo];
            if (getter == null)
            {
                getter = DynamicMethodFactory.CreatePropertyGetter(propertyInfo);
                s_getterDict[propertyInfo] = getter;
            }

            return getter(obj);
        }

        public static void FastSetValue2(this PropertyInfo propertyInfo, object obj, object value)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            SetValueDelegate setter = (SetValueDelegate)s_setterDict[propertyInfo];
            if (setter == null)
            {
                setter = DynamicMethodFactory.CreatePropertySetter(propertyInfo);
                s_setterDict[propertyInfo] = setter;
            }

            setter(obj, value);
        }


        public static object FastInvoke2(this MethodInfo methodInfo, object obj, params object[] parameters)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            MethodDelegate invoker = (MethodDelegate)s_methodDict[methodInfo];
            if (invoker == null)
            {
                invoker = DynamicMethodFactory.CreateMethod(methodInfo);
                s_methodDict[methodInfo] = invoker;
            }

            return invoker(obj, parameters);
        }
    }

    #endregion


    #region methodInfo 处理反射优化


    /// <summary>
    /// 一些扩展方法，用于快速调用方法，它们都可以优化反射性能。
    /// </summary>
    public static class MethodExtensions
    {
        /// <summary>
        /// 根据指定的MethodInfo以及参数数组，快速调用相关的方法。
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="obj"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object FastInvoke(this MethodInfo methodInfo, object obj, params object[] parameters)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            IInvokeMethod method = MethodInvokerFactory.GetMethodInvokerWrapper(methodInfo);
            return method.Invoke(obj, parameters);
        }
    }


    /// <summary>
    /// 定义了通用的方法调用接口
    /// </summary>
    public interface IInvokeMethod
    {
        object Invoke(object target, object[] parameters);
    }

    // 增加这个方法完全是为了简化ReflectMethodBase的继承类的实现代码
    internal interface IBindMethod
    {
        void BindMethod(MethodInfo method);
    }

    internal class CommonInvokerWrapper : IInvokeMethod
    {
        private MethodInfo _method;

        public CommonInvokerWrapper(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            _method = method;
        }

        public object Invoke(object target, object[] parameters)
        {
            if (_method.ReturnType == typeof(void))
            {
                _method.Invoke(target, parameters);
                return null;
            }

            return _method.Invoke(target, parameters);
        }
    }

    /// <summary>
    /// 为了简化实现IInvokeMethod接口的抽象类，继承类只需要重写InvokeInternal方法即可。
    /// </summary>
    /// <typeparam name="TDelegate"></typeparam>
    public abstract class ReflectMethodBase<TDelegate> : IInvokeMethod, IBindMethod where TDelegate : class
    {
        protected TDelegate _caller;
        protected object _returnValue;

        public void BindMethod(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (method.IsStatic)
                _caller = Delegate.CreateDelegate(typeof(TDelegate), method) as TDelegate;
            else
                _caller = Delegate.CreateDelegate(typeof(TDelegate), null, method) as TDelegate;
        }

        public object Invoke(object target, object[] parameters)
        {
            if (_caller == null)
                throw new InvalidOperationException("在调用Invoke之前没有调用BindMethod方法。");

            InvokeInternal(target, parameters);
            return _returnValue;
        }
        protected abstract void InvokeInternal(object target, object[] parameters);
    }
    /// <summary>
    /// 创建IInvokeMethod实例的工厂类
    /// </summary>
    public static class MethodInvokerFactory
    {
        private static readonly Hashtable s_dict = Hashtable.Synchronized(new Hashtable(10240));

        private static readonly Dictionary<string, Type> s_genericTypeDefinitions;

        static MethodInvokerFactory()
        {
            Type reflectMethodBase = typeof(ReflectMethodBase<>).GetGenericTypeDefinition();

            s_genericTypeDefinitions = (from t in typeof(MethodInvokerFactory).Assembly.GetExportedTypes()
                                        where t.BaseType != null
                                        && t.BaseType.IsGenericType
                                        && t.BaseType.GetGenericTypeDefinition() == reflectMethodBase
                                        select t).ToDictionary(x => x.Name);

            // 说明：这个工厂还有一种设计方法，
            // 直接分析类型的基类，检查是不是从ReflectMethodBase<>继承过来的，
            // 再分析类型参数中的委托的类型参数，从而得知这个类型可用于处理哪类方法的优化，
            // 并可以生成KEY，这样就不必与类型的名字有关了。
            // 但这种方法也有麻烦问题：由于每个实现类的类名没有名字上的约束，有可能生成相同的KEY，
            // 因为不同的类型都可以用于某一类方法的优化的，KEY就自然相同了。
            // 也正因为这个原因，CreateMethodWrapper 方法在生成KEY时，需要每个实现类的名字符合一定的约束条件。
        }

        internal static IInvokeMethod GetMethodInvokerWrapper(MethodInfo methodInfo)
        {
            IInvokeMethod method = (IInvokeMethod)s_dict[methodInfo];
            if (method == null)
            {
                method = CreateMethodInvokerWrapper(methodInfo);
                s_dict[methodInfo] = method;
            }

            return method;
        }

        /// <summary>
        /// 根据指定的MethodInfo对象创建相应的IInvokeMethod实例。
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static IInvokeMethod CreateMethodInvokerWrapper(MethodInfo method)
        {
            // 在这个类型的静态构造方法中，我已将所有能优化反射调用的泛型找出来，保存在s_genericTypeDefinitions中。
            // 这个工厂方法将根据：
            //	    1. 方法是否有返回值，
            //	    2. 方法是静态的，还是实例的，
            //	    3. 方法有多少个参数
            // 来查找能优化指定方法的那个泛型类型。

            // 不过，s_genericTypeDefinitions保存的泛型定义，属于开放泛型，
            // 工厂方法还要根据指定方法来填充类型参数，最后创建特定的泛型实例。

            if (method == null)
                throw new ArgumentNullException("method");

            ParameterInfo[] pameters = method.GetParameters();

            // 1. 首先根据指定方法的签名计算缓存键值
            string key = null;
            if (method.ReturnType == typeof(void))
            {
                if (method.IsStatic)
                {
                    if (pameters.Length == 0)
                        key = "StaticActionWrapper";
                    else
                        key = "StaticActionWrapper`" + pameters.Length.ToString();
                }
                else
                    key = "ActionWrapper`" + (pameters.Length + 1).ToString();
            }
            else
            {
                if (method.IsStatic)
                    key = "StaticFunctionWrapper`" + (pameters.Length + 1).ToString();
                else
                    key = "FunctionWrapper`" + (pameters.Length + 2).ToString();
            }

            // 2. 查找缓存，获取泛型定义
            Type genericTypeDefinition;
            if (s_genericTypeDefinitions.TryGetValue(key, out genericTypeDefinition) == false)
                // 如果找不到一个泛型类型，就返回下面这个通用的类型。
                // 下面这个类型将不会优化反射调用。
                return new CommonInvokerWrapper(method);


            Type instanceType = null;
            if (genericTypeDefinition.IsGenericTypeDefinition)
            {
                // 3. 获取填充泛型定义的类型参数。
                List<Type> list = new List<Type>(pameters.Length + 2);
                if (method.IsStatic == false)
                    list.Add(method.DeclaringType);

                for (int i = 0; i < pameters.Length; i++)
                    list.Add(pameters[i].ParameterType);

                if (method.ReturnType != typeof(void))
                    list.Add(method.ReturnType);

                // 4. 将泛型定义转成封闭泛型。
                instanceType = genericTypeDefinition.MakeGenericType(list.ToArray());
            }
            else
                instanceType = genericTypeDefinition;

            // 5. 实例化IReflectMethod对象。
            IInvokeMethod instance = (IInvokeMethod)Activator.CreateInstance(instanceType);

            IBindMethod binder = instance as IBindMethod;
            if (binder != null)
                binder.BindMethod(method);

            return instance;
        }
    }

    #endregion


    #region 调用方法委托

    public delegate void InstanceAction<TTarget>(TTarget target);


    public delegate void StaticAction();


    public delegate TResult InstanceFunc<TTarget, TResult>(TTarget target);


    public delegate TResult StaticFunc<TResult>();

    #endregion

    #region  调用方法 第一组，无参数。

    public partial class ActionWrapper<TTarget> : ReflectMethodBase<InstanceAction<TTarget>>
    {
        protected override void InvokeInternal(object target, object[] parameters)
        {
            _caller((TTarget)target);
        }
    }

    public partial class StaticActionWrapper : ReflectMethodBase<StaticAction>
    {
        protected override void InvokeInternal(object target, object[] parameters)
        {
            _caller();
        }
    }

    public partial class FunctionWrapper<TTarget, TResult> : ReflectMethodBase<InstanceFunc<TTarget, TResult>>
    {
        protected override void InvokeInternal(object target, object[] parameters)
        {
            _returnValue = _caller((TTarget)target);
        }
    }

    public partial class StaticFunctionWrapper<TResult> : ReflectMethodBase<StaticFunc<TResult>>
    {
        protected override void InvokeInternal(object target, object[] parameters)
        {
            _returnValue = _caller();
        }
    }

    #endregion

}



using System;
using System.Collections.Generic;
using System.Reflection;

namespace  STAROCK.Event
{
    public static class EventManager
    {
        private static readonly Dictionary<int, Delegate> EventLikeDelegates = new Dictionary<int, Delegate>();

        /// <summary>
        /// 向事件注册委托。
        /// </summary>
        /// <param name="id">用于标识事件的识别号。</param>
        /// <param name="delegate">要向事件注册的委托。</param>
        /// <exception cref="ArgumentException">被 <paramref name="id"/> 标识的事件和 <paramref name="delegate"/> 都不为 <see langword="null"/>，且它们不是相同委托类型的实例。</exception>
        public static void Register(int id, Delegate @delegate)
        {
            if (@delegate == null)
            {
                return;
            }
            lock (EventLikeDelegates)
            {
                if (EventLikeDelegates.TryGetValue(id, out var eventLikeDelegate))
                {
                    @delegate = Delegate.Combine(eventLikeDelegate, @delegate);
                }
                EventLikeDelegates[id] = @delegate;
            }
        }

        /// <summary>
        /// 从事件取消注册委托。
        /// </summary>
        /// <param name="id">用于标识事件的识别号。</param>
        /// <param name="delegate">要从事件取消注册的委托。</param>
        /// <exception cref="ArgumentException">被 <paramref name="id"/> 标识的事件和 <paramref name="delegate"/> 都不为 <see langword="null"/>，且它们不是相同委托类型的实例。</exception>
        public static void Unregister(int id, Delegate @delegate)
        {
            if (@delegate == null)
            {
                return;
            }
            lock (EventLikeDelegates)
            {
                if (!EventLikeDelegates.TryGetValue(id, out var eventLikeDelegate))
                {
                    return;
                }
                if ((@delegate = Delegate.Remove(eventLikeDelegate, @delegate)) != null)
                {
                    EventLikeDelegates[id] = @delegate;
                }
                else
                {
                    EventLikeDelegates.Remove(id);
                }
            }
        }

        /// <summary>
        /// 发送事件。
        /// </summary>
        /// <param name="id">用于标识事件的识别号。</param>
        /// <param name="args">参数。</param>
        /// <returns>如果存在被 <paramref name="id"/> 标识的事件，则为调用该事件的返回值；否则为 <see langword="null"/>。</returns>
        /// <exception cref="TargetParameterCountException"><paramref name="args"/> 中参数的数量不合法。</exception>
        /// <exception cref="ArgumentException"><paramref name="args"/> 中参数的顺序或类型不合法。</exception>
        public static object Send(int id, params object[] args)
        {
            Delegate eventLikeDelegate;
            lock (EventLikeDelegates)
            {
                if (!EventLikeDelegates.TryGetValue(id, out eventLikeDelegate))
                {
                    return null;
                }
            }
            return eventLikeDelegate.DynamicInvoke(args);
        }

        /// <summary>
        /// 发送事件。
        /// </summary>
        /// <param name="id">用于标识事件的识别号。</param>
        /// <param name="args">参数。</param>
        /// <typeparam name="T">返回值的类型。</typeparam>
        /// <returns>如果存在被 <paramref name="id"/> 标识的事件，则为调用该事件的返回值转换为 <typeparamref name="T"/>（如果为 <see langword="null"/> 则为 <typeparamref name="T"/> 的默认值）后的结果；否则为 <typeparamref name="T"/> 的默认值。</returns>
        /// <exception cref="TargetParameterCountException"><paramref name="args"/> 中参数的数量不合法。</exception>
        /// <exception cref="ArgumentException"><paramref name="args"/> 中参数的顺序或类型不合法。</exception>
        /// <exception cref="InvalidCastException">无法将返回值转换为 <typeparamref name="T"/>。</exception>
        public static T Send<T>(int id, params object[] args)
        {
            Delegate eventLikeDelegate;
            lock (EventLikeDelegates)
            {
                if (!EventLikeDelegates.TryGetValue(id, out eventLikeDelegate))
                {
                    return default;
                }
            }
            return (T) eventLikeDelegate.DynamicInvoke(args);
        }

        /// <summary>
        /// 发送事件。
        /// </summary>
        /// <param name="id">用于标识事件的识别号。</param>
        /// <param name="args">参数。</param>
        /// <returns>如果存在被 <paramref name="id"/> 标识的事件，则为该事件的委托列表按顺序调用后再将各个返回值组合而成的数组；否则为长度为 0 的数组。</returns>
        /// <exception cref="TargetParameterCountException"><paramref name="args"/> 中参数的数量不合法。</exception>
        /// <exception cref="ArgumentException"><paramref name="args"/> 中参数的顺序或类型不合法。</exception>
        public static object[] Sends(int id, params object[] args)
        {
            Delegate eventLikeDelegate;
            lock (EventLikeDelegates)
            {
                if (!EventLikeDelegates.TryGetValue(id, out eventLikeDelegate))
                {
                    return Array.Empty<object>();
                }
            }
            var delegates    = eventLikeDelegate.GetInvocationList();
            var length       = delegates.Length;
            var returnValues = new object[length];
            for (var i = 0; i < length; i++)
            {
                returnValues[i] = delegates[i].DynamicInvoke(args);
            }
            return returnValues;
        }

        /// <summary>
        /// 发送事件。
        /// </summary>
        /// <param name="id">用于标识事件的识别号。</param>
        /// <param name="args">参数。</param>
        /// <typeparam name="T">返回值的类型。</typeparam>
        /// <returns>如果存在被 <paramref name="id"/> 标识的事件，则为该事件的委托列表按顺序调用后再将各个返回值转换为 <typeparamref name="T"/>（如果为 <see langword="null"/> 则为 <typeparamref name="T"/> 的默认值）后再组合而成的数组；否则为长度为 0 的数组。</returns>
        /// <exception cref="TargetParameterCountException"><paramref name="args"/> 中参数的数量不合法。</exception>
        /// <exception cref="ArgumentException"><paramref name="args"/> 中参数的顺序或类型不合法。</exception>
        /// <exception cref="InvalidCastException">在调用被 <paramref name="id"/> 标识的事件的委托列表时，无法将其中的某一个返回值转换为 <typeparamref name="T"/>。</exception>
        public static T[] Sends<T>(int id, params object[] args)
        {
            Delegate eventLikeDelegate;
            lock (EventLikeDelegates)
            {
                if (!EventLikeDelegates.TryGetValue(id, out eventLikeDelegate))
                {
                    return Array.Empty<T>();
                }
            }
            var delegates    = eventLikeDelegate.GetInvocationList();
            var length       = delegates.Length;
            var returnValues = new T[length];
            for (var i = 0; i < length; i++)
            {
                returnValues[i] = (T) delegates[i].DynamicInvoke(args);
            }
            return returnValues;
        }

        /// <summary>
        /// 清除 <see cref="EventManager"/> 中的所有事件。
        /// </summary>
        public static void Clear()
        {
            lock (EventLikeDelegates)
            {
                EventLikeDelegates.Clear();
            }
        }
    }

} 
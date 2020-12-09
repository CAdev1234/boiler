using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using sand_data_ctrl;
using System.Data;
using System.Data.Common;
using System.Reflection;
using EntityFramework.Extensions;



namespace ykmWeb.Dal
{
    /// <summary>
    /// 包含ADO和EF的数据库操作类
    /// </summary>
    /// <typeparam name="T">模型类</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ykmWebDbContext s;
        public BaseRepository(ykmWebDbContext ykmWebDbContext)
        {
            s = ykmWebDbContext;
        }

        public T add(T entity)
        {

            s.Entry<T>(entity).State = EntityState.Added;
            s.SaveChanges();
            return entity;


        }

        public int count(Expression<Func<T, bool>> predicate)
        {

            if (predicate == null)
            {
                return s.Set<T>().Count();
            }
            else
            {
                return s.Set<T>().Count(predicate);
            }

        }


        public void edit(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updatawhere)
        {

            T t = (T)Activator.CreateInstance(typeof(T));
            s.Set<T>().Where(predicate).Update(updatawhere);

        }

        public void addAll(List<T> l)
        {

            T t = (T)Activator.CreateInstance(typeof(T));
            s.Set<T>().AddRange(l);
            s.SaveChanges();

        }

        public bool edit(T entity)
        {

            T t = (T)Activator.CreateInstance(typeof(T));
            s.Set<T>().Attach(entity);
            s.Entry(entity).State = EntityState.Modified;

            return s.SaveChanges() > 0;


        }

        public bool del(T entity)
        {

            s.Set<T>().Attach(entity);
            s.Entry<T>(entity).State = EntityState.Deleted;
            return s.SaveChanges() > 0;


        }

        public bool del_all(Expression<Func<T, bool>> where)
        {

            var temp = s.Set<T>().Where(where);
            foreach (var item in temp)
            {
                s.Entry<T>(item).State = EntityState.Deleted;
            }
            return s.SaveChanges() > 0;

        }

        public bool exist(Expression<Func<T, bool>> anyLambda)
        {

            return s.Set<T>().Any(anyLambda);


        }

        public T find(Expression<Func<T, bool>> whereLambda)
        {

            T _entity = s.Set<T>().FirstOrDefault<T>(whereLambda);
            return _entity;


        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable<T></returns>
        private IQueryable<T> OrderBy(IQueryable<T> source, string propertyName, bool isAsc)
        {
            if (source == null) throw new ArgumentNullException("source", "不能为空");
            if (string.IsNullOrEmpty(propertyName)) return source;
            var _parameter = Expression.Parameter(source.ElementType);
            var _property = Expression.Property(_parameter, propertyName);
            if (_property == null) throw new ArgumentNullException("propertyName", "属性不存在");
            var _lambda = Expression.Lambda(_property, _parameter);
            var _methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var _resultExpression = Expression.Call(typeof(Queryable), _methodName, new Type[] { source.ElementType, _property.Type }, source.Expression, Expression.Quote(_lambda));
            return source.Provider.CreateQuery<T>(_resultExpression);
        }


        /// <summary>
        /// 多个排序通用方法
        /// </summary>
        /// <typeparam name="Tkey">排序字段</typeparam>
        /// <param name="data">要排序的数据</param>
        /// <param name="orderWhereAndIsDesc">字典集合(排序条件,是否倒序)</param>
        /// <returns>排序后的集合</returns>
        private IQueryable<T> OrderBy(IQueryable<T> data, params OrderModelField[] orderByExpression)
        {
            //创建表达式变量参数
            var parameter = Expression.Parameter(typeof(T), "o");

            if (orderByExpression != null && orderByExpression.Length > 0)
            {
                for (int i = 0; i < orderByExpression.Length; i++)
                {
                    //根据属性名获取属性
                    var property = typeof(T).GetProperty(orderByExpression[i].propertyName);
                    //创建一个访问属性的表达式
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    string OrderName = "";
                    if (i > 0)
                    {
                        OrderName = orderByExpression[i].IsDESC ? "ThenByDescending" : "ThenBy";
                    }
                    else
                        OrderName = orderByExpression[i].IsDESC ? "OrderByDescending" : "OrderBy";

                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(T), property.PropertyType },
                        data.Expression, Expression.Quote(orderByExp));

                    data = data.Provider.CreateQuery<T>(resultExp);
                }
            }
            return data;
        }

        public IQueryable<T> FindList(Expression<Func<T, bool>> whereLadmbda, int topnum, OrderModelField[] orderByExpression)
        {
            IQueryable<T> _list;
            if (whereLadmbda != null)
            {
                _list = s.Set<T>().AsNoTracking().Where<T>(whereLadmbda);
            }
            else
            {
                _list = s.Set<T>().AsNoTracking().AsQueryable<T>();
            }


            if (topnum > 0)
            {
                _list = OrderBy(_list, orderByExpression).Take<T>(topnum);
            }
            else
            {
                _list = OrderBy(_list, orderByExpression);
            }

            return _list;
        }

        public IQueryable<T> FindListPage(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, OrderModelField[] orderByExpression)
        {
            IQueryable<T> _list;
            if (whereLamdba != null)
            {
                _list = s.Set<T>().AsNoTracking().Where(whereLamdba);
            }
            else
            {
                _list = s.Set<T>().AsNoTracking().AsQueryable();
            }
            totalRecord = _list.Count();
            _list = OrderBy(_list, orderByExpression).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return _list;
        }

    }
}

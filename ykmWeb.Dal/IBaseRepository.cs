using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ykmWeb.Dal
{
    interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         T add(T entity);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="l"></param>
        void addAll(List<T> l);
        /// <summary>
        /// 返回数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int count(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updatawhere"></param>
        void edit(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updatawhere);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool edit(T entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool del(T entity);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool del_all(Expression<Func<T, bool>> where);
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        bool exist(Expression<Func<T, bool>> anyLambda);
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        T find(Expression<Func<T, bool>> whereLambda);
       

    }
}

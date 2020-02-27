﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Domain.Uow
{
    /// <summary>
    /// Used to complete a unit of work. This interface can not be injected or directly used. Use
    /// <see cref="IUnitOfWorkManager"/> instead.
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// Completes this unit of work. It saves all changes and commit transaction if exists.
        /// </summary>
        void Complete();

        /// <summary>
        /// Completes this unit of work. It saves all changes and commit transaction if exists.
        /// </summary>
        Task CompleteAsync();
    }
}
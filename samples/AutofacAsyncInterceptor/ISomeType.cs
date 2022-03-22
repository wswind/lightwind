﻿using System.Threading.Tasks;

namespace AutofacAsyncInterceptor
{
    public interface ISomeType
    {
        Task<string> ShowAsyncWithReturnValue(string input);
        Task ShowAsync(string input);
        void ShowSynchronous(string input);
        ValueTask ShowValueTask(string input);
        ValueTask<string> ShowValueTaskWithReturnValue(string input);
    }
}

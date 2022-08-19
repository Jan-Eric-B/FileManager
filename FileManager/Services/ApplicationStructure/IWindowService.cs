using System;

namespace FileManager.Services.ApplicationStructure
{
    public interface IWindowService
    {
        public void Show(Type windowType);

        public T Show<T>() where T : class;
    }
}
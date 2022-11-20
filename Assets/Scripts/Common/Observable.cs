using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class Observable<T>
    {
        [SerializeField] private T _value;

        public Observable() { }
        public Observable(T value) => _value = value;

        public event Action<T> Changed;

        public T Value { get => _value; set => Set(value); }

        public void Set(T value)
        {
            _value = value;
            Changed?.Invoke(_value);
        }

        public static implicit operator T(Observable<T> value) => value.Value;
        public static implicit operator Observable<T>(T value) => new Observable<T>(value);
    }
}
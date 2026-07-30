#nullable enable

using System;
using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Методы расширения для <see cref="GameObject"/> и <see cref="Component"/>,
    /// упрощающие поиск и добавление компонентов.
    /// </summary>
    public static class Extensions_ComponentMethods
    {
        #region HasComponent

        /// <summary>
        /// Проверяет, прикреплён ли к данному игровому объекту компонент указанного типа.
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента.</typeparam>
        /// <param name="self">Игровой объект, на котором выполняется проверка.</param>
        /// <returns><see langword="true"/>, если компонент найден; иначе <see langword="false"/>.</returns>
        public static bool HasComponent<TComponent>(this GameObject self) => self.GetComponent<TComponent>() != null;

        /// <summary>
        /// Проверяет, прикреплён ли к данному игровому объекту компонент указанного типа.
        /// </summary>
        /// <param name="self">Игровой объект, на котором выполняется проверка.</param>
        /// <param name="type">Тип компонента.</param>
        /// <returns><see langword="true"/>, если компонент найден; иначе <see langword="false"/>.</returns>
        public static bool HasComponent(this GameObject self, Type type) => self.GetComponent(type) != null;

        /// <summary>
        /// Проверяет, прикреплён ли к тому же игровому объекту, что и данный компонент, компонент указанного типа.
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента.</typeparam>
        /// <param name="self">Компонент, на игровом объекте которого выполняется проверка.</param>
        /// <returns><see langword="true"/>, если компонент найден; иначе <see langword="false"/>.</returns>
        public static bool HasComponent<TComponent>(this Component self) => self.GetComponent<TComponent>() != null;

        /// <summary>
        /// Проверяет, прикреплён ли к тому же игровому объекту, что и данный компонент, компонент указанного типа.
        /// </summary>
        /// <param name="self">Компонент, на игровом объекте которого выполняется проверка.</param>
        /// <param name="type">Тип компонента.</param>
        /// <returns><see langword="true"/>, если компонент найден; иначе <see langword="false"/>.</returns>
        public static bool HasComponent(this Component self, Type type) => self.GetComponent(type) != null;

        #endregion

        #region GetOrAddComponent

        /// <summary>
        /// Возвращает компонент указанного типа, прикреплённый к данному игровому объекту.
        /// Если компонент отсутствует, он будет добавлен.
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента, унаследованный от <see cref="Component"/>.</typeparam>
        /// <param name="self">Игровой объект, на котором ищется или добавляется компонент.</param>
        /// <returns>Существующий или только что добавленный компонент.</returns>
        public static TComponent GetOrAddComponent<TComponent>(this GameObject self) where TComponent : Component
        {
            if (self.TryGetComponent(out TComponent component))
            {
                return component;
            }
            return self.AddComponent<TComponent>();
        }

        /// <summary>
        /// Возвращает компонент, реализующий указанный интерфейс или базовый тип.
        /// Если такой компонент отсутствует, добавляется компонент типа <typeparamref name="TAddedComponent"/>.
        /// </summary>
        /// <typeparam name="TComponent">Тип, по которому производится поиск (интерфейс или базовый класс).</typeparam>
        /// <typeparam name="TAddedComponent">Тип добавляемого компонента. Должен быть наследником <see cref="Component"/> и реализовывать <typeparamref name="TComponent"/>.</typeparam>
        /// <param name="self">Игровой объект, на котором ищется или добавляется компонент.</param>
        /// <returns>Существующий компонент, реализующий <typeparamref name="TComponent"/>, либо только что добавленный компонент типа <typeparamref name="TAddedComponent"/>.</returns>
        public static TComponent GetOrAddComponent<TComponent, TAddedComponent>(this GameObject self) where TAddedComponent : Component, TComponent
        {
            if (self.TryGetComponent(out TComponent component))
            {
                return component;
            }
            return self.AddComponent<TAddedComponent>();
        }

        /// <summary>
        /// Возвращает компонент указанного типа, прикреплённый к данному игровому объекту.
        /// Если компонент отсутствует, он будет добавлен.
        /// </summary>
        /// <param name="self">Игровой объект, на котором ищется или добавляется компонент.</param>
        /// <param name="type">Тип искомого и, при необходимости, добавляемого компонента.</param>
        /// <returns>Существующий или только что добавленный компонент.</returns>
        public static Component GetOrAddComponent(this GameObject self, Type type)
        {
            if (self.TryGetComponent(type, out Component component))
            {
                return component;
            }
            return self.AddComponent(type);
        }

        /// <summary>
        /// Возвращает компонент указанного типа, прикреплённый к данному игровому объекту.
        /// Если компонент отсутствует, добавляется компонент типа <paramref name="addedType"/>.
        /// </summary>
        /// <param name="self">Игровой объект, на котором ищется или добавляется компонент.</param>
        /// <param name="type">Тип, по которому производится поиск.</param>
        /// <param name="addedType">Тип добавляемого компонента. Должен быть наследником <see cref="Component"/> и совместимым с <paramref name="type"/>.</param>
        /// <returns>Существующий компонент, совместимый с <paramref name="type"/>, либо только что добавленный компонент типа <paramref name="addedType"/>.</returns>
        public static Component GetOrAddComponent(this GameObject self, Type type, Type addedType)
        {
            if (self.TryGetComponent(type, out Component component))
            {
                return component;
            }
            return self.AddComponent(addedType);
        }

        /// <summary>
        /// Возвращает компонент указанного типа, прикреплённый к тому же игровому объекту, что и данный компонент.
        /// Если компонент отсутствует, он будет добавлен.
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента, унаследованный от <see cref="Component"/>.</typeparam>
        /// <param name="self">Компонент, на игровом объекте которого ищется или добавляется компонент.</param>
        /// <returns>Существующий или только что добавленный компонент.</returns>
        public static TComponent GetOrAddComponent<TComponent>(this Component self) where TComponent : Component
        {
            if (self.TryGetComponent(out TComponent component))
            {
                return component;
            }
            return self.gameObject.AddComponent<TComponent>();
        }

        /// <summary>
        /// Возвращает компонент, реализующий указанный интерфейс или базовый тип, на том же игровом объекте.
        /// Если такой компонент отсутствует, добавляется компонент типа <typeparamref name="TAddedComponent"/>.
        /// </summary>
        /// <typeparam name="TComponent">Тип, по которому производится поиск (интерфейс или базовый класс).</typeparam>
        /// <typeparam name="TAddedComponent">Тип добавляемого компонента. Должен быть наследником <see cref="Component"/> и реализовывать <typeparamref name="TComponent"/>.</typeparam>
        /// <param name="self">Компонент, на игровом объекте которого ищется или добавляется компонент.</param>
        /// <returns>Существующий компонент, реализующий <typeparamref name="TComponent"/>, либо только что добавленный компонент типа <typeparamref name="TAddedComponent"/>.</returns>
        public static TComponent GetOrAddComponent<TComponent, TAddedComponent>(this Component self) where TAddedComponent : Component, TComponent
        {
            if (self.TryGetComponent(out TComponent component))
            {
                return component;
            }
            return self.gameObject.AddComponent<TAddedComponent>();
        }

        /// <summary>
        /// Возвращает компонент указанного типа, прикреплённый к тому же игровому объекту, что и данный компонент.
        /// Если компонент отсутствует, он будет добавлен.
        /// </summary>
        /// <param name="self">Компонент, на игровом объекте которого ищется или добавляется компонент.</param>
        /// <param name="type">Тип искомого и, при необходимости, добавляемого компонента.</param>
        /// <returns>Существующий или только что добавленный компонент.</returns>
        public static Component GetOrAddComponent(this Component self, Type type)
        {
            if (self.TryGetComponent(type, out Component component))
            {
                return component;
            }
            return self.gameObject.AddComponent(type);
        }

        /// <summary>
        /// Возвращает компонент указанного типа, прикреплённый к тому же игровому объекту, что и данный компонент.
        /// Если компонент отсутствует, добавляется компонент типа <paramref name="addedType"/>.
        /// </summary>
        /// <param name="self">Компонент, на игровом объекте которого ищется или добавляется компонент.</param>
        /// <param name="type">Тип, по которому производится поиск.</param>
        /// <param name="addedType">Тип добавляемого компонента. Должен быть наследником <see cref="Component"/> и совместимым с <paramref name="type"/>.</param>
        /// <returns>Существующий компонент, совместимый с <paramref name="type"/>, либо только что добавленный компонент типа <paramref name="addedType"/>.</returns>
        public static Component GetOrAddComponent(this Component self, Type type, Type addedType)
        {
            if (self.TryGetComponent(type, out Component component))
            {
                return component;
            }
            return self.gameObject.AddComponent(addedType);
        }

        #endregion

        #region GetComponentInParentOnly

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов (исключая сам исходный объект).
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TComponent">Тип искомого компонента.</typeparam>
        /// <param name="self">Компонент, для которого выполняется поиск вверх по иерархии (родитель и выше).</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static TComponent? GetComponentInParentOnly<TComponent>(this Component self)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent<TComponent>();
        }

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов, исключая исходный объект,
        /// с возможностью включения неактивных объектов.
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TComponent">Тип искомого компонента.</typeparam>
        /// <param name="self">Компонент, для которого выполняется поиск вверх по иерархии.</param>
        /// <param name="includeInactive">Следует ли включать в поиск неактивные объекты.</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static TComponent? GetComponentInParentOnly<TComponent>(this Component self, bool includeInactive)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent<TComponent>(includeInactive);
        }

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов (исключая сам исходный объект).
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <param name="self">Компонент, для которого выполняется поиск вверх по иерархии.</param>
        /// <param name="type">Тип искомого компонента.</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static Component? GetComponentInParentOnly(this Component self, Type type)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent(type);
        }

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов, исключая исходный объект,
        /// с возможностью включения неактивных объектов.
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <param name="self">Компонент, для которого выполняется поиск вверх по иерархии.</param>
        /// <param name="type">Тип искомого компонента.</param>
        /// <param name="includeInactive">Следует ли включать в поиск неактивные объекты.</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static Component? GetComponentInParentOnly(this Component self, Type type, bool includeInactive)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent(type, includeInactive);
        }

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов (исключая сам исходный объект).
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TComponent">Тип искомого компонента.</typeparam>
        /// <param name="self">Игровой объект, от родителя которого начинается поиск вверх.</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static TComponent? GetComponentInParentOnly<TComponent>(this GameObject self)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent<TComponent>();
        }

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов, исключая исходный объект,
        /// с возможностью включения неактивных объектов.
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TComponent">Тип искомого компонента.</typeparam>
        /// <param name="self">Игровой объект, от родителя которого начинается поиск вверх.</param>
        /// <param name="includeInactive">Следует ли включать в поиск неактивные объекты.</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static TComponent? GetComponentInParentOnly<TComponent>(this GameObject self, bool includeInactive)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent<TComponent>(includeInactive);
        }

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов (исключая сам исходный объект).
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <param name="self">Игровой объект, от родителя которого начинается поиск вверх.</param>
        /// <param name="type">Тип искомого компонента.</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static Component? GetComponentInParentOnly(this GameObject self, Type type)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent(type);
        }

        /// <summary>
        /// Ищет компонент указанного типа только среди родительских объектов, исключая исходный объект,
        /// с возможностью включения неактивных объектов.
        /// Если родитель отсутствует, возвращает <see langword="null"/>.
        /// </summary>
        /// <param name="self">Игровой объект, от родителя которого начинается поиск вверх.</param>
        /// <param name="type">Тип искомого компонента.</param>
        /// <param name="includeInactive">Следует ли включать в поиск неактивные объекты.</param>
        /// <returns>Найденный компонент или <see langword="null"/>.</returns>
        public static Component? GetComponentInParentOnly(this GameObject self, Type type, bool includeInactive)
        {
            Transform parent = self.transform.parent;
            if (parent == null)
            {
                return default;
            }
            return parent.GetComponentInParent(type, includeInactive);
        }

        #endregion
    }
}

namespace nucs.SystemCore.Binding {
    /// <summary>
    /// Binds an object, creating events over it's setting and it's getting. (see 'Watcher' for further implementation)
    /// </summary>
    public class Bindable<T> {
        public delegate void ItemModifiedHandler(T _new, T _old);
        public delegate void ItemRequestedHandler(T output);
        public event ItemModifiedHandler ItemModified;
        public event ItemRequestedHandler ItemRequested;

        private T _instance;
        public T Item {
            get {
                Version++;
                if (ItemRequested != null)
                    ItemRequested(_instance);
                return _instance;
            }
            set {
                Version++;
                if (ItemModified != null)
                    ItemModified(value, _instance);
                _instance = value; 
            }
        }
        private long Version;

        internal Bindable(T item) {
            Version++;
            _instance = item;
        }

        public static explicit operator T(Bindable<T> binder) {
            return binder._instance;
        }

        #region Object Overrides

        public bool Equals(Bindable<T> obj) {
            return Version.Equals(obj.Version) && _instance.Equals(obj._instance);
        }

        public bool Equals(T obj) {
            return _instance.Equals(obj);
        }

        public bool Equals(Watcher<T> obj) {
            return _instance.Equals(obj.Item);
        }

        public override bool Equals(object obj) {
            if (typeof (Bindable<T>) == obj.GetType()) {
                var bindable = obj as Bindable<T>;
                return bindable != null && (_instance.Equals(bindable.Item) && bindable.Version.Equals(Version));
            }
            if (typeof (T) != obj.GetType())
                return false;
            return _instance.Equals(obj) ;
        }
        
        public override int GetHashCode() {
            return _instance.GetHashCode();
        }

        public override string ToString() {
            return _instance.ToString();
        }
        #endregion

    }

    /// <summary>
    /// Given 'T instace', it will be replace every time 'T WatchingOver' is modified too.
    /// </summary>
    public class Watcher<T> {
        private T _instance;
        public T Item {
            get { return _instance; }
            set { _instance = value; }
        }
        public Bindable<T> WatchesOver { get; private set; }

        public bool IsWatching { get; set; }
        public void ToggleWatcher() {
            IsWatching = !IsWatching;
            if (IsWatching)
                WatchesOver.ItemModified += watcherAction;
            else
                WatchesOver.ItemModified -= watcherAction;
        }
        private readonly Bindable<T>.ItemModifiedHandler watcherAction;
        internal Watcher(T instance, Bindable<T> over) {
            IsWatching = true;
            watcherAction = (_new, _old) => Item = _new;

            _instance = instance;
            WatchesOver = over;
            over.ItemModified += watcherAction;
        }

        public static explicit operator T(Watcher<T> binder) {
            return binder._instance;
        }

        public static explicit operator Bindable<T>(Watcher<T> binder) {
            return binder.WatchesOver;
        }
    }

    public static class Binding {
        /// <summary>
        /// Binds an object, creating events over it's setting and it's getting. (see 'Watcher' for further implementation)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Bindable<T> TurnBindable<T>(this T instance) {
            return new Bindable<T>(instance);
        }

        /// <summary>
        /// Given 'T instace', it will be replace every time 'Bindable T over' is modified too.
        /// </summary>
        /// <typeparam name="T">Type that this will be based over</typeparam>
        /// <param name="instance">Master object, going to change after binder</param>
        /// <param name="over">bindable object, 'instance' will follow it's changes</param>
        /// <returns></returns>
        public static Watcher<T> WatchOver<T>(this T instance, Bindable<T> over) {
            return new Watcher<T>(instance, over);
        }
    }
}

namespace System
{
    //TODO: document
    public delegate void OpenAction<in TThis>(TThis @this);
    public delegate void OpenAction<in TThis, in T>(TThis @this, T arg);
    public delegate void OpenAction<in TThis, in T, in T2>(TThis @this, T arg, T2 arg2);
    public delegate void OpenAction<in TThis, in T, in T2, in T3>(TThis @this, T arg, T2 arg2, T3 arg3);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5, in T6>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    public delegate void OpenAction<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    public delegate R OpenFunc<in TThis, out R>(TThis @this);
    public delegate R OpenFunc<in TThis, in T, out R>(TThis @this, T arg);
    public delegate R OpenFunc<in TThis, in T, in T2, out R>(TThis @this, T arg, T2 arg2);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, out R>(TThis @this, T arg, T2 arg2, T3 arg3);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, in T6, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    public delegate R OpenFunc<in TThis, in T, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out R>(TThis @this, T arg, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
}
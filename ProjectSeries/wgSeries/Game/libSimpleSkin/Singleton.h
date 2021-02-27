#pragma once

template<class T>
class Singleton
{
public:	
	virtual ~Singleton(void){}

	static T * Instance();
	static void ForceToDelete();

private:
	Singleton(void){}

	T						m_Instance;
	static Singleton<T>*	g_singleton;
};

template<class T>
Singleton<T>* Singleton<T>::g_singleton = NULL;

template<class T>
T* Singleton<T>::Instance()
{
	if ( g_singleton == NULL )
	{
		g_singleton = new Singleton<T>;
	}

	return &g_singleton->m_Instance;
}

template<class T>
void Singleton<T>::ForceToDelete()
{
	if ( g_singleton != NULL )
	{
		delete g_singleton;
		g_singleton = NULL;
	}
}

#include "pch-cpp.hpp"





struct VirtualActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename R>
struct VirtualFuncInvoker0
{
	typedef R (*Func)(void*, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename T1>
struct InvokerActionInvoker1;
template <typename T1>
struct InvokerActionInvoker1<T1*>
{
	static inline void Invoke (Il2CppMethodPointer methodPtr, const RuntimeMethod* method, void* obj, T1* p1)
	{
		void* params[1] = { p1 };
		method->invoker_method(methodPtr, method, obj, params, params[0]);
	}
};
template <typename T1, typename T2>
struct InvokerActionInvoker2;
template <typename T1, typename T2>
struct InvokerActionInvoker2<T1*, T2*>
{
	static inline void Invoke (Il2CppMethodPointer methodPtr, const RuntimeMethod* method, void* obj, T1* p1, T2* p2)
	{
		void* params[2] = { p1, p2 };
		method->invoker_method(methodPtr, method, obj, params, params[1]);
	}
};
template <typename T1, typename T2, typename T3>
struct InvokerActionInvoker3;
template <typename T1, typename T2, typename T3>
struct InvokerActionInvoker3<T1*, T2*, T3*>
{
	static inline void Invoke (Il2CppMethodPointer methodPtr, const RuntimeMethod* method, void* obj, T1* p1, T2* p2, T3* p3)
	{
		void* params[3] = { p1, p2, p3 };
		method->invoker_method(methodPtr, method, obj, params, params[2]);
	}
};
template <typename R, typename T1>
struct InvokerFuncInvoker1;
template <typename R, typename T1>
struct InvokerFuncInvoker1<R, T1*>
{
	static inline R Invoke (Il2CppMethodPointer methodPtr, const RuntimeMethod* method, void* obj, T1* p1)
	{
		R ret;
		void* params[1] = { p1 };
		method->invoker_method(methodPtr, method, obj, params, &ret);
		return ret;
	}
};
template <typename R, typename T1, typename T2>
struct InvokerFuncInvoker2;
template <typename R, typename T1, typename T2>
struct InvokerFuncInvoker2<R, T1*, T2*>
{
	static inline R Invoke (Il2CppMethodPointer methodPtr, const RuntimeMethod* method, void* obj, T1* p1, T2* p2)
	{
		R ret;
		void* params[2] = { p1, p2 };
		method->invoker_method(methodPtr, method, obj, params, &ret);
		return ret;
	}
};
template <typename R, typename T1, typename T2, typename T3, typename T4>
struct InvokerFuncInvoker4;
template <typename R, typename T1, typename T2, typename T3, typename T4>
struct InvokerFuncInvoker4<R, T1*, T2*, T3*, T4>
{
	static inline R Invoke (Il2CppMethodPointer methodPtr, const RuntimeMethod* method, void* obj, T1* p1, T2* p2, T3* p3, T4 p4)
	{
		R ret;
		void* params[4] = { p1, p2, p3, &p4 };
		method->invoker_method(methodPtr, method, obj, params, &ret);
		return ret;
	}
};
template <typename R>
struct ConstrainedFuncInvoker0
{
	static inline R Invoke (RuntimeClass* type, const RuntimeMethod* constrainedMethod, void* boxBuffer, void* obj)
	{
		R ret;
		il2cpp_codegen_runtime_constrained_call(type, constrainedMethod, boxBuffer, obj, NULL, &ret);
		return ret;
	}
};

struct Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87;
struct ConcurrentDictionary_2_t0ADC4F91250F9F331F8482562C831FF3920619C9;
struct Dictionary_2_t6682243BDABA638FCBE4F1D9875B5B1455A7686E;
struct ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583;
struct ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD;
struct Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB;
struct Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5;
struct Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA;
struct Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821;
struct Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA;
struct Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00;
struct Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F;
struct Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858;
struct Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD;
struct Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541;
struct Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B;
struct Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB;
struct Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B;
struct Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0;
struct Func_2_t93FE63D487003DC89C264F70099E05071B9C1169;
struct Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8;
struct Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C;
struct Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19;
struct Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595;
struct Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0;
struct Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F;
struct Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9;
struct Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821;
struct IEnumerable_1_tFCB3E41202F3CC65C7DE9B40C22F196A081E84D6;
struct IEnumerable_1_t2F3CDAFD157993698650E145CA662718328EA696;
struct IEnumerable_1_tCE758D940790D6D0D56B457E522C195F8C413AF2;
struct IEnumerable_1_t71A46277DBD73BD4009B2B20885D2B7057593A1A;
struct IEnumerable_1_t5359DEC999AA35C7E2DE775B0455A4760550ED7F;
struct IEnumerable_1_tF95C9E01A913DD50575531C8305932628663D9E9;
struct IEnumerable_1_t29E7244AE33B71FA0981E50D5BC73B7938F35C66;
struct IEnumerator_1_t48F4900A82CAF9059C2FEB04454888B64E155362;
struct IEnumerator_1_t8367D305629D4B00EFD835BD8049D28EF078FFD0;
struct IEnumerator_1_tD6A90A7446DA8E6CF865EDFBBF18C1200BB6D452;
struct IEnumerator_1_t239F6ACD0FC026E7FA70965FDE161517CD367AED;
struct IEnumerator_1_t6999C610E1C05F2C90151CD5C41E24528ACB3255;
struct IEnumerator_1_t43D2E4BA9246755F293DFA74F001FB1A70A648FD;
struct IEnumerator_1_t75CB2681E18F7F2791528FA2CA60361FDB5DA08D;
struct Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5;
struct Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610;
struct Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07;
struct Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E;
struct Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D;
struct Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA;
struct Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0;
struct List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D;
struct List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29;
struct List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A;
struct List_1_tC2D82F414D0061F9548408EE99757FA88F71C7BD;
struct List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A;
struct WhereEnumerableIterator_1_tC79E61165AF53521C305E751B0FFF9FDBE427D6D;
struct WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9;
struct WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A;
struct WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374;
struct WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C;
struct WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4;
struct WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736;
struct WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484;
struct WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F;
struct WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7;
struct WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC;
struct WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336;
struct WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9;
struct WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5;
struct WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730;
struct WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9;
struct WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E;
struct WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F;
struct WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507;
struct WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A;
struct WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960;
struct WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6;
struct WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896;
struct WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3;
struct Wrapper_tDCD6E9769A559E52C6D10132E041EA3EFF52BC98;
struct WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234;
struct WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1;
struct WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D;
struct WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7;
struct WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789;
struct WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0;
struct WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8;
struct WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7;
struct XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A;
struct XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D;
struct XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072;
struct XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0;
struct YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339;
struct EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635;
struct EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4;
struct ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031;
struct CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB;
struct DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771;
struct Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C;
struct IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832;
struct JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA;
struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918;
struct StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF;
struct SubstringU5BU5D_t9973080FDB519ED771E9FB3E6FDC90DF45211B77;
struct TypeU5BU5D_t97234E1129B564EB38B8D85CAC2AD8B5B9522FFB;
struct Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD;
struct __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC;
struct JsonValueU5BU5D_tCC9BDCF1DE43F923B72BEF6FE9AB4FBAEF445DB3;
struct LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263;
struct Binder_t91BFCE95A7057FADF4D8A1A342AFE52872246235;
struct Delegate_t;
struct DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E;
struct EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3;
struct IDictionary_t6D03155AF1FA9083817AA5B6AD7DEEACC26AB220;
struct JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6;
struct Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3;
struct MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553;
struct MethodInfo_t;
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C;
struct OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C;
struct SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6;
struct String_t;
struct Type_t;
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;
struct WitResponseArray_tAC1DCF75C6C2FAB0A70A7DB879A0F35999BAB30A;
struct WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C;
struct EventBetterWorker_tC1AA937EE3E2B99071A1CE8C189B6EDE3138726C;

IL2CPP_EXTERN_C RuntimeClass* Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Sorting_tBB4ACAADCAA21EA710DD3998A0614ABDEF8FD8A6_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675_il2cpp_TypeInfo_var;
struct Delegate_t_marshaled_com;
struct Delegate_t_marshaled_pinvoke;
struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com;

struct EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635;
struct EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4;
struct DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771;
struct Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C;
struct JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA;
struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918;
struct Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD;
struct __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC;
struct LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263;

IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5  : public RuntimeObject
{
	int32_t ___threadId;
	int32_t ___state;
	bool ___current;
};
struct Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07  : public RuntimeObject
{
	int32_t ___threadId;
	int32_t ___state;
	int32_t ___current;
};
struct Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA  : public RuntimeObject
{
	int32_t ___threadId;
	int32_t ___state;
	RuntimeObject* ___current;
};
struct Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0 : public RuntimeObject {};
struct List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D  : public RuntimeObject
{
	ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ____items;
	int32_t ____size;
	int32_t ____version;
	RuntimeObject* ____syncRoot;
};
struct List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29  : public RuntimeObject
{
	SubstringU5BU5D_t9973080FDB519ED771E9FB3E6FDC90DF45211B77* ____items;
	int32_t ____size;
	int32_t ____version;
	RuntimeObject* ____syncRoot;
};
struct List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A  : public RuntimeObject
{
	__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ____items;
	int32_t ____size;
	int32_t ____version;
	RuntimeObject* ____syncRoot;
};
struct List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A  : public RuntimeObject
{
	JsonValueU5BU5D_tCC9BDCF1DE43F923B72BEF6FE9AB4FBAEF445DB3* ____items;
	int32_t ____size;
	int32_t ____version;
	RuntimeObject* ____syncRoot;
};
struct WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507  : public RuntimeObject
{
	WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___responseNode;
	String_t* ___id;
	String_t* ___name;
	String_t* ___role;
	int32_t ___start;
	int32_t ___end;
	String_t* ___type;
	String_t* ___body;
	int32_t ___value;
	float ___confidence;
	bool ___hasData;
	WitResponseArray_tAC1DCF75C6C2FAB0A70A7DB879A0F35999BAB30A* ___entities;
};
struct WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A  : public RuntimeObject
{
	WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___responseNode;
	String_t* ___id;
	String_t* ___name;
	String_t* ___role;
	int32_t ___start;
	int32_t ___end;
	String_t* ___type;
	String_t* ___body;
	RuntimeObject* ___value;
	float ___confidence;
	bool ___hasData;
	WitResponseArray_tAC1DCF75C6C2FAB0A70A7DB879A0F35999BAB30A* ___entities;
};
struct WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960  : public RuntimeObject
{
	WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___responseNode;
	String_t* ___id;
	String_t* ___name;
	String_t* ___role;
	int32_t ___start;
	int32_t ___end;
	String_t* ___type;
	String_t* ___body;
	float ___value;
	float ___confidence;
	bool ___hasData;
	WitResponseArray_tAC1DCF75C6C2FAB0A70A7DB879A0F35999BAB30A* ___entities;
};
struct WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6 : public RuntimeObject {};
struct WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896  : public RuntimeObject
{
	int32_t ____headIndex;
	int32_t ____tailIndex;
	ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ____array;
	int32_t ____mask;
	int32_t ____addTakeCount;
	int32_t ____stealCount;
	int32_t ____currentOp;
	bool ____frozen;
	WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* ____nextQueue;
	int32_t ____ownerThreadId;
};
struct WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3  : public RuntimeObject
{
	int32_t ____headIndex;
	int32_t ____tailIndex;
	__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ____array;
	int32_t ____mask;
	int32_t ____addTakeCount;
	int32_t ____stealCount;
	int32_t ____currentOp;
	bool ____frozen;
	WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* ____nextQueue;
	int32_t ____ownerThreadId;
};
struct Wrapper_tDCD6E9769A559E52C6D10132E041EA3EFF52BC98 : public RuntimeObject {};
struct XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A  : public RuntimeObject
{
	Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* ____buckets;
	EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* ____entries;
	int32_t ____numEntries;
	ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* ____extractKey;
};
struct XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D  : public RuntimeObject
{
	Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* ____buckets;
	EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* ____entries;
	int32_t ____numEntries;
	ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* ____extractKey;
};
struct XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072  : public RuntimeObject
{
	XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* ____state;
};
struct XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0  : public RuntimeObject
{
	XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* ____state;
};
struct YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339  : public RuntimeObject
{
	Delegate_t* ___handler;
	List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* ___U3CMessagesU3Ek__BackingField;
	EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3* ___EventBetterInstance;
};
struct EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3  : public RuntimeObject
{
	ConcurrentDictionary_2_t0ADC4F91250F9F331F8482562C831FF3920619C9* ___s_entries;
	List_1_tC2D82F414D0061F9548408EE99757FA88F71C7BD* ___s_entriesList;
	EventBetterWorker_tC1AA937EE3E2B99071A1CE8C189B6EDE3138726C* ___s_worker;
};
struct JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6  : public RuntimeObject
{
	bool ___U3CCanReadU3Ek__BackingField;
	bool ___U3CCanWriteU3Ek__BackingField;
};
struct MemberInfo_t  : public RuntimeObject
{
};
struct String_t  : public RuntimeObject
{
	int32_t ____stringLength;
	Il2CppChar ____firstChar;
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C  : public RuntimeObject
{
};
struct Entry_t6E72C722C46949F140550D563C1F41E304752583 
{
	RuntimeObject* ___Value;
	int32_t ___HashCode;
	int32_t ___Next;
};
typedef Il2CppFullySharedGenericStruct Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E;
typedef Il2CppFullySharedGenericStruct Enumerator_tF5AC6CD19D283FBD724440520CEE68FE2602F7AF;
struct WhereEnumerableIterator_1_tC79E61165AF53521C305E751B0FFF9FDBE427D6D  : public Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5
{
	RuntimeObject* ___source;
	Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB* ___predicate;
	RuntimeObject* ___enumerator;
};
struct WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A  : public Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07
{
	RuntimeObject* ___source;
	Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA* ___predicate;
	RuntimeObject* ___enumerator;
};
struct WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4  : public Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA
{
	RuntimeObject* ___source;
	Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00* ___predicate;
	RuntimeObject* ___enumerator;
};
struct WhereEnumerableIterator_1_t8B24528558F527941435C4FE1D046216FE4F277B : public RuntimeObject {};
struct WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44 
{
	Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___m_Data;
	int32_t ___m_Start;
	int32_t ___m_Length;
};
struct WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809 
{
	__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___m_Data;
	int32_t ___m_Start;
	int32_t ___m_Length;
};
struct WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87 
{
	LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___m_Data;
	int32_t ___m_Start;
	int32_t ___m_Length;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value;
};
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17 
{
	Il2CppChar ___m_value;
};
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2  : public ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F
{
};
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2_marshaled_pinvoke
{
};
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2_marshaled_com
{
};
struct Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C 
{
	int32_t ___m_value;
};
struct Int64_t092CFB123BE63C28ACDAF65C68F21A526050DBA3 
{
	int64_t ___m_value;
};
struct IntPtr_t 
{
	void* ___m_value;
};
struct InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 
{
	String_t* ___m_StringOriginalCase;
	String_t* ___m_StringLowerCase;
};
struct InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735_marshaled_pinvoke
{
	char* ___m_StringOriginalCase;
	char* ___m_StringLowerCase;
};
struct InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735_marshaled_com
{
	Il2CppChar* ___m_StringOriginalCase;
	Il2CppChar* ___m_StringLowerCase;
};
struct Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D 
{
	float ___m_XMin;
	float ___m_YMin;
	float ___m_Width;
	float ___m_Height;
};
struct RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8 
{
	int32_t ___m_XMin;
	int32_t ___m_YMin;
	int32_t ___m_Width;
	int32_t ___m_Height;
};
struct Single_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C 
{
	float ___m_value;
};
struct SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675 
{
	int32_t ____count;
};
struct Substring_t2E16755269E6716C22074D6BC0A9099915E67849 
{
	String_t* ___m_String;
	int32_t ___m_Index;
	int32_t ___m_Length;
};
struct Substring_t2E16755269E6716C22074D6BC0A9099915E67849_marshaled_pinvoke
{
	char* ___m_String;
	int32_t ___m_Index;
	int32_t ___m_Length;
};
struct Substring_t2E16755269E6716C22074D6BC0A9099915E67849_marshaled_com
{
	Il2CppChar* ___m_String;
	int32_t ___m_Index;
	int32_t ___m_Length;
};
struct Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 
{
	float ___x;
	float ___y;
};
struct Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A 
{
	int32_t ___m_X;
	int32_t ___m_Y;
};
struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 
{
	float ___x;
	float ___y;
	float ___z;
};
struct Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 
{
	int32_t ___m_X;
	int32_t ___m_Y;
	int32_t ___m_Z;
};
struct Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 
{
	float ___x;
	float ___y;
	float ___z;
	float ___w;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 
{
	uint16_t ___visibleLightIndex;
	uint16_t ___lightBufferIndex;
	Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* ___light;
};
struct LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2_marshaled_pinvoke
{
	uint16_t ___visibleLightIndex;
	uint16_t ___lightBufferIndex;
	Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* ___light;
};
struct LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2_marshaled_com
{
	uint16_t ___visibleLightIndex;
	uint16_t ___lightBufferIndex;
	Light_t1E68479B7782AF2050FAA02A5DC612FD034F18F3* ___light;
};
struct Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 
{
	List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ____list;
	int32_t ____index;
	int32_t ____version;
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ____current;
};
struct Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D  : public RuntimeObject
{
	int32_t ___threadId;
	int32_t ___state;
	InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 ___current;
};
struct WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336 : public RuntimeObject {};
struct Delegate_t  : public RuntimeObject
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	RuntimeObject* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	bool ___method_is_virtual;
};
struct Delegate_t_marshaled_pinvoke
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	Il2CppIUnknown* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	int32_t ___method_is_virtual;
};
struct Delegate_t_marshaled_com
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	Il2CppIUnknown* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	int32_t ___method_is_virtual;
};
struct Exception_t  : public RuntimeObject
{
	String_t* ____className;
	String_t* ____message;
	RuntimeObject* ____data;
	Exception_t* ____innerException;
	String_t* ____helpURL;
	RuntimeObject* ____stackTrace;
	String_t* ____stackTraceString;
	String_t* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	RuntimeObject* ____dynamicMethods;
	int32_t ____HResult;
	String_t* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct Exception_t_marshaled_pinvoke
{
	char* ____className;
	char* ____message;
	RuntimeObject* ____data;
	Exception_t_marshaled_pinvoke* ____innerException;
	char* ____helpURL;
	Il2CppIUnknown* ____stackTrace;
	char* ____stackTraceString;
	char* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	Il2CppIUnknown* ____dynamicMethods;
	int32_t ____HResult;
	char* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	Il2CppSafeArray* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct Exception_t_marshaled_com
{
	Il2CppChar* ____className;
	Il2CppChar* ____message;
	RuntimeObject* ____data;
	Exception_t_marshaled_com* ____innerException;
	Il2CppChar* ____helpURL;
	Il2CppIUnknown* ____stackTrace;
	Il2CppChar* ____stackTraceString;
	Il2CppChar* ____remoteStackTraceString;
	int32_t ____remoteStackIndex;
	Il2CppIUnknown* ____dynamicMethods;
	int32_t ____HResult;
	Il2CppChar* ____source;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces;
	Il2CppSafeArray* ___native_trace_ips;
	int32_t ___caught_in_unmanaged;
};
struct Int32Enum_tCBAC8BA2BFF3A845FA599F303093BBBA374B6F0C 
{
	int32_t ___value__;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C  : public RuntimeObject
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
	intptr_t ___m_CachedPtr;
};
struct RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B 
{
	intptr_t ___value;
};
struct HandlerFlags_tF24A3D4FC9DFD1E6D5C10643CFD8A8A95C995917 
{
	int32_t ___value__;
};
struct JsonString_tE22CDDA995FEFF514F3F334C93B6AB31B49773CB 
{
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___text;
	bool ___hasEscapes;
};
struct JsonString_tE22CDDA995FEFF514F3F334C93B6AB31B49773CB_marshaled_pinvoke
{
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849_marshaled_pinvoke ___text;
	int32_t ___hasEscapes;
};
struct JsonString_tE22CDDA995FEFF514F3F334C93B6AB31B49773CB_marshaled_com
{
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849_marshaled_com ___text;
	int32_t ___hasEscapes;
};
struct JsonValueType_t36BA339F107E5E9C0966C45F896E27F3BCC5A2AB 
{
	int32_t ___value__;
};
struct Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E  : public RuntimeObject
{
	int32_t ___threadId;
	int32_t ___state;
	int32_t ___current;
};
struct WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C  : public Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D
{
	RuntimeObject* ___source;
	Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA* ___predicate;
	RuntimeObject* ___enumerator;
};
struct WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484  : public Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07
{
	List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___source;
	Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___predicate;
	Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* ___selector;
	Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 ___enumerator;
};
struct WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7  : public Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D
{
	List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___source;
	Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___predicate;
	Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* ___selector;
	Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 ___enumerator;
};
struct WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC  : public Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA
{
	List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___source;
	Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___predicate;
	Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* ___selector;
	Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 ___enumerator;
};
struct InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA 
{
	Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C* ___U3CInstanceU3Ek__BackingField;
	Type_t* ___U3CTypeU3Ek__BackingField;
	int32_t ___U3CInstanceIdU3Ek__BackingField;
};
struct InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA_marshaled_pinvoke
{
	Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke ___U3CInstanceU3Ek__BackingField;
	Type_t* ___U3CTypeU3Ek__BackingField;
	int32_t ___U3CInstanceIdU3Ek__BackingField;
};
struct InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA_marshaled_com
{
	Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com* ___U3CInstanceU3Ek__BackingField;
	Type_t* ___U3CTypeU3Ek__BackingField;
	int32_t ___U3CInstanceIdU3Ek__BackingField;
};
struct MulticastDelegate_t  : public Delegate_t
{
	DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771* ___delegates;
};
struct MulticastDelegate_t_marshaled_pinvoke : public Delegate_t_marshaled_pinvoke
{
	Delegate_t_marshaled_pinvoke** ___delegates;
};
struct MulticastDelegate_t_marshaled_com : public Delegate_t_marshaled_com
{
	Delegate_t_marshaled_com** ___delegates;
};
struct SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295  : public Exception_t
{
};
struct Type_t  : public MemberInfo_t
{
	RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B ____impl;
};
struct JsonValue_t01DB320267C848E729A400EF2345979978F851D2 
{
	int32_t ___type;
	bool ___boolValue;
	double ___realValue;
	int64_t ___integerValue;
	JsonString_tE22CDDA995FEFF514F3F334C93B6AB31B49773CB ___stringValue;
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___arrayValue;
	Dictionary_2_t6682243BDABA638FCBE4F1D9875B5B1455A7686E* ___objectValue;
	RuntimeObject* ___anyValue;
};
struct JsonValue_t01DB320267C848E729A400EF2345979978F851D2_marshaled_pinvoke
{
	int32_t ___type;
	int32_t ___boolValue;
	double ___realValue;
	int64_t ___integerValue;
	JsonString_tE22CDDA995FEFF514F3F334C93B6AB31B49773CB_marshaled_pinvoke ___stringValue;
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___arrayValue;
	Dictionary_2_t6682243BDABA638FCBE4F1D9875B5B1455A7686E* ___objectValue;
	Il2CppIUnknown* ___anyValue;
};
struct JsonValue_t01DB320267C848E729A400EF2345979978F851D2_marshaled_com
{
	int32_t ___type;
	int32_t ___boolValue;
	double ___realValue;
	int64_t ___integerValue;
	JsonString_tE22CDDA995FEFF514F3F334C93B6AB31B49773CB_marshaled_com ___stringValue;
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___arrayValue;
	Dictionary_2_t6682243BDABA638FCBE4F1D9875B5B1455A7686E* ___objectValue;
	Il2CppIUnknown* ___anyValue;
};
struct Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87  : public MulticastDelegate_t
{
};
struct Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB 
{
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ____list;
	int32_t ____index;
	int32_t ____version;
	JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ____current;
};
struct ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583  : public MulticastDelegate_t
{
};
struct ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD  : public MulticastDelegate_t
{
};
struct Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB  : public MulticastDelegate_t
{
};
struct Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5  : public MulticastDelegate_t
{
};
struct Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA  : public MulticastDelegate_t
{
};
struct Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821  : public MulticastDelegate_t
{
};
struct Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA  : public MulticastDelegate_t
{
};
struct Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00  : public MulticastDelegate_t
{
};
struct Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F  : public MulticastDelegate_t
{
};
struct Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858  : public MulticastDelegate_t
{
};
struct Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD  : public MulticastDelegate_t
{
};
struct Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541  : public MulticastDelegate_t
{
};
struct Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B  : public MulticastDelegate_t
{
};
struct Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB  : public MulticastDelegate_t
{
};
struct Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B  : public MulticastDelegate_t
{
};
struct Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0  : public MulticastDelegate_t
{
};
struct Func_2_t93FE63D487003DC89C264F70099E05071B9C1169  : public MulticastDelegate_t
{
};
struct Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8  : public MulticastDelegate_t
{
};
struct Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C  : public MulticastDelegate_t
{
};
struct Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19  : public MulticastDelegate_t
{
};
struct Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595  : public MulticastDelegate_t
{
};
struct Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0  : public MulticastDelegate_t
{
};
struct Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F  : public MulticastDelegate_t
{
};
struct Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9  : public MulticastDelegate_t
{
};
struct Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821  : public MulticastDelegate_t
{
};
struct Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610  : public RuntimeObject
{
	int32_t ___threadId;
	int32_t ___state;
	InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA ___current;
};
struct WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374  : public Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E
{
	RuntimeObject* ___source;
	Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821* ___predicate;
	RuntimeObject* ___enumerator;
};
struct WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F  : public Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E
{
	List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___source;
	Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___predicate;
	Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* ___selector;
	Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 ___enumerator;
};
struct WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234  : public MulticastDelegate_t
{
};
struct WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1  : public MulticastDelegate_t
{
};
struct WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D  : public MulticastDelegate_t
{
};
struct WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7  : public MulticastDelegate_t
{
};
struct WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789  : public MulticastDelegate_t
{
};
struct WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0  : public MulticastDelegate_t
{
};
struct WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8  : public MulticastDelegate_t
{
};
struct WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7  : public MulticastDelegate_t
{
};
struct ArithmeticException_t07E77822D0007642BC8959A671E70D1F33C84FEA  : public SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295
{
};
struct WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9  : public Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610
{
	RuntimeObject* ___source;
	Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5* ___predicate;
	RuntimeObject* ___enumerator;
};
struct WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736  : public Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610
{
	List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___source;
	Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___predicate;
	Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* ___selector;
	Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 ___enumerator;
};
struct WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9  : public Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5
{
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___source;
	Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___predicate;
	Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___selector;
	Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB ___enumerator;
};
struct WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5  : public Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610
{
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___source;
	Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___predicate;
	Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* ___selector;
	Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB ___enumerator;
};
struct WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730  : public Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07
{
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___source;
	Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___predicate;
	Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* ___selector;
	Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB ___enumerator;
};
struct WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9  : public Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E
{
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___source;
	Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___predicate;
	Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* ___selector;
	Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB ___enumerator;
};
struct WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E  : public Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D
{
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___source;
	Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___predicate;
	Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* ___selector;
	Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB ___enumerator;
};
struct WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F  : public Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA
{
	List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___source;
	Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___predicate;
	Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* ___selector;
	Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB ___enumerator;
};
struct OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C  : public ArithmeticException_t07E77822D0007642BC8959A671E70D1F33C84FEA
{
};
struct List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D_StaticFields
{
	ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___s_emptyArray;
};
struct List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29_StaticFields
{
	SubstringU5BU5D_t9973080FDB519ED771E9FB3E6FDC90DF45211B77* ___s_emptyArray;
};
struct List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A_StaticFields
{
	__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___s_emptyArray;
};
struct List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A_StaticFields
{
	JsonValueU5BU5D_tCC9BDCF1DE43F923B72BEF6FE9AB4FBAEF445DB3* ___s_emptyArray;
};
struct String_t_StaticFields
{
	String_t* ___Empty;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17_StaticFields
{
	ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___s_categoryForLatin1;
};
struct IntPtr_t_StaticFields
{
	intptr_t ___Zero;
};
struct Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D_StaticFields
{
	Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D ___kZero;
};
struct RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8_StaticFields
{
	RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8 ___kZero;
};
struct SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675_StaticFields
{
	int32_t ___SpinCountforSpinBeforeWait;
};
struct Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7_StaticFields
{
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___zeroVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___oneVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___upVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___downVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___leftVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___rightVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___positiveInfinityVector;
	Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7 ___negativeInfinityVector;
};
struct Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A_StaticFields
{
	Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A ___s_Zero;
	Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A ___s_One;
	Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A ___s_Up;
	Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A ___s_Down;
	Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A ___s_Left;
	Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A ___s_Right;
};
struct Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2_StaticFields
{
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___zeroVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___oneVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___upVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___downVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___leftVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___rightVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___forwardVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___backVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___positiveInfinityVector;
	Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2 ___negativeInfinityVector;
};
struct Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376_StaticFields
{
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_Zero;
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_One;
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_Up;
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_Down;
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_Left;
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_Right;
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_Forward;
	Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376 ___s_Back;
};
struct Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_StaticFields
{
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___zeroVector;
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___oneVector;
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___positiveInfinityVector;
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___negativeInfinityVector;
};
struct LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2_StaticFields
{
	Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___s_CompareByCookieSize;
	Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___s_CompareByBufferIndex;
};
struct Type_t_StaticFields
{
	Binder_t91BFCE95A7057FADF4D8A1A342AFE52872246235* ___s_defaultBinder;
	Il2CppChar ___Delimiter;
	TypeU5BU5D_t97234E1129B564EB38B8D85CAC2AD8B5B9522FFB* ___EmptyTypes;
	RuntimeObject* ___Missing;
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterAttribute;
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterName;
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterNameIgnoreCase;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif
struct JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA  : public RuntimeArray
{
	ALIGN_FIELD (8) JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6* m_Items[1];

	inline JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, JsonConverter_t1EC0651CD277ABEFD390D833BEAB25A034223BE6* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
struct Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD  : public RuntimeArray
{
	ALIGN_FIELD (8) Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 m_Items[1];

	inline Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 value)
	{
		m_Items[index] = value;
	}
};
struct __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC  : public RuntimeArray
{
	ALIGN_FIELD (8) uint8_t m_Items[1];

	inline uint8_t* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + il2cpp_array_calc_byte_offset(this, index);
	}
	inline uint8_t* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + il2cpp_array_calc_byte_offset(this, index);
	}
};
struct LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263  : public RuntimeArray
{
	ALIGN_FIELD (8) LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 m_Items[1];

	inline LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___light), (void*)NULL);
	}
	inline LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___light), (void*)NULL);
	}
};
struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918  : public RuntimeArray
{
	ALIGN_FIELD (8) RuntimeObject* m_Items[1];

	inline RuntimeObject* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline RuntimeObject** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, RuntimeObject* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline RuntimeObject* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline RuntimeObject** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, RuntimeObject* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
struct DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771  : public RuntimeArray
{
	ALIGN_FIELD (8) Delegate_t* m_Items[1];

	inline Delegate_t* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline Delegate_t** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, Delegate_t* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline Delegate_t* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline Delegate_t** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, Delegate_t* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
struct Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C  : public RuntimeArray
{
	ALIGN_FIELD (8) int32_t m_Items[1];

	inline int32_t GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline int32_t* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, int32_t value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline int32_t GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline int32_t* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, int32_t value)
	{
		m_Items[index] = value;
	}
};
struct EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635  : public RuntimeArray
{
	ALIGN_FIELD (8) Entry_t6E72C722C46949F140550D563C1F41E304752583 m_Items[1];

	inline Entry_t6E72C722C46949F140550D563C1F41E304752583 GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline Entry_t6E72C722C46949F140550D563C1F41E304752583* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, Entry_t6E72C722C46949F140550D563C1F41E304752583 value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___Value), (void*)NULL);
	}
	inline Entry_t6E72C722C46949F140550D563C1F41E304752583 GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline Entry_t6E72C722C46949F140550D563C1F41E304752583* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, Entry_t6E72C722C46949F140550D563C1F41E304752583 value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)&((m_Items + index)->___Value), (void*)NULL);
	}
};
struct EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4  : public RuntimeArray
{
	ALIGN_FIELD (8) uint8_t m_Items[1];

	inline uint8_t* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + il2cpp_array_calc_byte_offset(this, index);
	}
	inline uint8_t* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + il2cpp_array_calc_byte_offset(this, index);
	}
};


IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Iterator_1__ctor_mB4FD6E6C2748201577FA407A6347A551413E8DA0_gshared (Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m74371F82A28BD893528CD04F939FEBF8DBAB8208_gshared (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264_gshared (List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Substring_t2E16755269E6716C22074D6BC0A9099915E67849 Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_gshared_inline (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_gshared_inline (Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA Func_2_Invoke_mD2BF1A07861A43FD1AC9DAAB47815991AD1A959F_gshared_inline (Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06_gshared (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereEnumerableIterator_1__ctor_mCD2AE447BF5F2DB9BD2FCC12898977FE4F004AB7_gshared (WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9* __this, RuntimeObject* ___0_source, Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5* ___1_predicate, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Iterator_1__ctor_m426D8BD95B00FDA5C5E8FAE956FCB74E54BC7CC7_gshared (Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mE284E07CB05A30CB3CD67A6B975FFBB56EA2B829_gshared (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_mF4550AAF07822B89D9CEDD14EA0A726DC11B8487_gshared_inline (Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereEnumerableIterator_1__ctor_mA7F4C6508A043D99B8288E43867D576F4697E4EE_gshared (WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A* __this, RuntimeObject* ___0_source, Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA* ___1_predicate, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Iterator_1__ctor_m9788F4EF50A0094F325E620BFC1D32ED3DAE63BB_gshared (Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mA035EEA0C242B3BEB72DA969A7D95966BC3C3DCF_gshared (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_m168B819DF4F932C2AD05BF95F897984BE8B5B6C0_gshared_inline (Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereEnumerableIterator_1__ctor_m6C929ED5C1A2B964D41834542D001D44C5784AE1_gshared (WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374* __this, RuntimeObject* ___0_source, Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821* ___1_predicate, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Iterator_1__ctor_m1B09BFBB8B83E605D7C7487F0E6A4986CE070943_gshared (Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m76DFD3D8B5B62444383A84BE159C61CBE99D98DA_gshared (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 Func_2_Invoke_mA098B5996B6AE11EADA0A2F2DE377135468CEBAC_gshared_inline (Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereEnumerableIterator_1__ctor_m0C64291A42967A645028F01B6069873BFB0831E4_gshared (WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C* __this, RuntimeObject* ___0_source, Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA* ___1_predicate, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Iterator_1__ctor_m3E47867714E05673E54C6B73D9242F5FFADA1F63_gshared (Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mDEC1BA11C8B63FF5BC0568D2F273C66867A0FC83_gshared (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* Func_2_Invoke_m3C9BCAF1F1F54BB85749F5001D1DFDBEAB600AC8_gshared_inline (Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereEnumerableIterator_1__ctor_m8C0DA4CDA5431C03561F67C4393BB18CDD891F01_gshared (WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4* __this, RuntimeObject* ___0_source, Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00* ___1_predicate, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Iterator_1__ctor_m9CBF3685A44F015ED364E012FC67ACDC7935DD96_gshared (Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mF1B366622F25F80FCC3B4A235B88A3E1F8D3C81B_gshared (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13_gshared (List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR JsonValue_t01DB320267C848E729A400EF2345979978F851D2 Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_gshared_inline (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_gshared_inline (Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF_gshared (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereEnumerableIterator_1__ctor_m2672E4EFA1C94475E8D626D4793081BAA021C25C_gshared (WhereEnumerableIterator_1_tC79E61165AF53521C305E751B0FFF9FDBE427D6D* __this, RuntimeObject* ___0_source, Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB* ___1_predicate, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m9D9A35DD876EEDD07A424ECC1BF4F949472E9B4C_gshared (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA Func_2_Invoke_m5FCD56659798D9D7163D543B4EFB36176A166CC2_gshared_inline (Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m2F45E03F13987C106503E1A03492C57DFF49FA6D_gshared (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_m321B177CDE142D1D55BBD1D63153186A54FEE249_gshared_inline (Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mC28B3C62105E92A56FF8F653549CD0EAF52EE6F7_gshared (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_m42E5D81BF1869A4411FD8979982060200B5C8644_gshared_inline (Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mA16961886A67BC49FFE65D6057D3BA28D407DF89_gshared (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 Func_2_Invoke_m7C8D770BA29067A536942979753FAB53ED84A348_gshared_inline (Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mDB747029572F65F32894254A0E779C445B0B1E15_gshared (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* ___2_selector, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* Func_2_Invoke_mCF21D6903C1C9AB285FB0E244CDE0E9ACAC429ED_gshared_inline (Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* JsonConvert_DeserializeIntoObject_TisRuntimeObject_m88B0EA93F53DC9CBB88A3E31C7D4648EC392CF9E_gshared (RuntimeObject* ___0_instance, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___1_jsonToken, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA* ___2_customConverters, bool ___3_suppressWarnings, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_mF7CEAE61925236C3A0BE9E92E9B97B65EF5DFFEF_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_m8B26C5F2B3AB3C05CB4ACDE5A17C6559BDEF503C_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 WorkSlice_1_get_Item_m31D876CE45C89B17D24E2D2EB6534C4150D72EE5_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, int32_t ___0_index, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_set_Item_m242025FA56F79603361582298B70AB2284A12067_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, int32_t ___0_index, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___1_value, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_mCDE5BAF472BC1BEBC9D091F532AC1A07D65DB0BC_gshared_inline (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_capacity_m514E3E1482974088A807521F9E2C481EB83C25F8_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Sorting_QuickSort_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mB8556CFA6B9237741AAA1ADF2EE68AA3696F9477_gshared (Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_data, int32_t ___1_start, int32_t ___2_end, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F* ___3_compare, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_Sort_m9DF994BABD5BF00DE8FEB14D82F7A0A9F6CAEE3D_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F* ___0_compare, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_mFEB81358558CEF0264CCE077535DB880EA2492BA_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_m7AC3CC7AABC83B76D23D2B94329DD4D0200156FA_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_get_Item_mFDEC427E08156ECD6879AD45AEE3618B43E3F726_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, int32_t ___0_index, Il2CppFullySharedGenericAny* il2cppRetVal, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_set_Item_m757F8BE76FAE27C149DE7C474A2B1845E08A5A0F_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, int32_t ___0_index, Il2CppFullySharedGenericAny ___1_value, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_m7212817506EACBA1AB0D914DE401C6FA05F0DD9D_gshared_inline (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_capacity_mCF0D603B7EC6E6037D0E1A14D8D0F49AD063E59E_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_Sort_mE2ED392BDF8F4F4141D7BF4C984EFE943F607A94_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9* ___0_compare, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_mC3BCAA8301A4E37DDB26811AAFFE1A3654FA47D0_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_m76CCC65E3DFB8B84A2154B65A79B56688F9B26A4_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 WorkSlice_1_get_Item_mFD8EE50B88077F3EF9BCEF97BD6D0352D2E8445D_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, int32_t ___0_index, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_set_Item_m070889CA2F62E82384BAB3CEF6D6E9AABF153663_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, int32_t ___0_index, LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 ___1_value, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_m0E5769CB5B052657E7327DCAD0F2CA104327D7D8_gshared_inline (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_capacity_m98A237D126494A11FF3C61211B36A790E4E8A3B0_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Sorting_QuickSort_TisLightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2_mD7019ED48D20810C1169430283118F96945E9450_gshared (LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_data, int32_t ___1_start, int32_t ___2_end, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___3_compare, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_Sort_m7EF532E936D55845DAAC606C0A214FE48EBF8584_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___0_compare, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkStealingQueue_get_DangerousCount_m7EA23F3971CF379A3C0A87B49FECB670CD7D738E_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* ExtractKeyDelegate_Invoke_m3214A3F92C2EB22088AB7738E8DFE1BE70925148_gshared_inline (ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* __this, RuntimeObject* ___0_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XHashtableState__ctor_m37E975C296D7D8CDF9ACAA9ABD7C4A0C82C427D3_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* ___0_extractKey, int32_t ___1_capacity, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_TryAdd_m951A9A914307495745E2B33DB0BA6583FB4060A1_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, RuntimeObject* ___0_value, RuntimeObject** ___1_newValue, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XHashtableState_ComputeHashCode_mD58A823B4A09F8DB93853EA9536E6B457B59F1B1_gshared (String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_FindEntry_m3AC0430564190D3FCAD9A8DB1DD0E725F7392A91_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, int32_t ___0_hashCode, String_t* ___1_key, int32_t ___2_index, int32_t ___3_count, int32_t* ___4_entryIndex, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_TryGetValue_m22BFB31B1A4E2CEDE4CA7133F4A0B50B9E514308_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, RuntimeObject** ___3_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* XHashtableState_Resize_mDB3BA5C327EF6A86F6CE58B59E1D7430E62532C5_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_gshared_inline (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t List_1_get_Count_m4407E4C389F22B8CEC282C15D56516658746C383_gshared_inline (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* List_1_get_Item_m33561245D64798C2AB07584C0EC4F240E4839A38_gshared (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, int32_t ___0_index, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Action_1__ctor_m2E1DFA67718FC1A0B6E5DFEB78831FFE9C059EB4_gshared (Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Delegate_t* EventBetter_RegisterInternal_TisRuntimeObject_TisRuntimeObject_m0C8C9474A4E38D813E4B1BC8B7A8F65A2ED7172D_gshared (EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3* __this, RuntimeObject* ___0_listener, Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87* ___1_handler, int32_t ___2_flags, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void List_1__ctor_m7F078BB342729BDF11327FD89D7872265328F690_gshared (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void YieldListener_1_set_Messages_mD81C51A83A5A04A77DB20F2FA7BA057016AFCC66_gshared_inline (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* ___0_value, const RuntimeMethod* method) ;
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void List_1_Add_mEBCF994CC3814631017F46A387B1A192ED6C85C7_gshared_inline (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, RuntimeObject* ___0_item, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1_Dispose_m9C4194F200BBADB8E122CBD8749141F82A3DE8F2_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1_OnMessage_mF071E8A97B1E58D5F5007B04FD9008F26196EEC7_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, RuntimeObject* ___0_msg, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_NO_INLINE IL2CPP_METHOD_ATTR void List_1_AddWithResize_m79A9BF770BEF9C06BE40D5401E55E375F2726CC4_gshared (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, RuntimeObject* ___0_item, const RuntimeMethod* method) ;

inline void Iterator_1__ctor_mB4FD6E6C2748201577FA407A6347A551413E8DA0 (Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610* __this, const RuntimeMethod* method)
{
	((  void (*) (Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*, const RuntimeMethod*))Iterator_1__ctor_mB4FD6E6C2748201577FA407A6347A551413E8DA0_gshared)(__this, method);
}
inline void WhereSelectListIterator_2__ctor_m74371F82A28BD893528CD04F939FEBF8DBAB8208 (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736*, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29*, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F*, Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_m74371F82A28BD893528CD04F939FEBF8DBAB8208_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264 (List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* __this, const RuntimeMethod* method)
{
	return ((  Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 (*) (List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29*, const RuntimeMethod*))List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264_gshared)(__this, method);
}
inline Substring_t2E16755269E6716C22074D6BC0A9099915E67849 Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_inline (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* __this, const RuntimeMethod* method)
{
	return ((  Substring_t2E16755269E6716C22074D6BC0A9099915E67849 (*) (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*, const RuntimeMethod*))Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_gshared_inline)(__this, method);
}
inline bool Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_inline (Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method)
{
	return ((  bool (*) (Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*))Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_gshared_inline)(__this, ___0_arg, method);
}
inline InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA Func_2_Invoke_mD2BF1A07861A43FD1AC9DAAB47815991AD1A959F_inline (Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method)
{
	return ((  InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA (*) (Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*))Func_2_Invoke_mD2BF1A07861A43FD1AC9DAAB47815991AD1A959F_gshared_inline)(__this, ___0_arg, method);
}
inline bool Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06 (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* __this, const RuntimeMethod* method)
{
	return ((  bool (*) (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*, const RuntimeMethod*))Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06_gshared)(__this, method);
}
inline void WhereEnumerableIterator_1__ctor_mCD2AE447BF5F2DB9BD2FCC12898977FE4F004AB7 (WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9* __this, RuntimeObject* ___0_source, Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5* ___1_predicate, const RuntimeMethod* method)
{
	((  void (*) (WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9*, RuntimeObject*, Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5*, const RuntimeMethod*))WhereEnumerableIterator_1__ctor_mCD2AE447BF5F2DB9BD2FCC12898977FE4F004AB7_gshared)(__this, ___0_source, ___1_predicate, method);
}
inline void Iterator_1__ctor_m426D8BD95B00FDA5C5E8FAE956FCB74E54BC7CC7 (Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07* __this, const RuntimeMethod* method)
{
	((  void (*) (Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*, const RuntimeMethod*))Iterator_1__ctor_m426D8BD95B00FDA5C5E8FAE956FCB74E54BC7CC7_gshared)(__this, method);
}
inline void WhereSelectListIterator_2__ctor_mE284E07CB05A30CB3CD67A6B975FFBB56EA2B829 (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484*, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29*, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F*, Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_mE284E07CB05A30CB3CD67A6B975FFBB56EA2B829_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline int32_t Func_2_Invoke_mF4550AAF07822B89D9CEDD14EA0A726DC11B8487_inline (Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*))Func_2_Invoke_mF4550AAF07822B89D9CEDD14EA0A726DC11B8487_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereEnumerableIterator_1__ctor_mA7F4C6508A043D99B8288E43867D576F4697E4EE (WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A* __this, RuntimeObject* ___0_source, Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA* ___1_predicate, const RuntimeMethod* method)
{
	((  void (*) (WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A*, RuntimeObject*, Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA*, const RuntimeMethod*))WhereEnumerableIterator_1__ctor_mA7F4C6508A043D99B8288E43867D576F4697E4EE_gshared)(__this, ___0_source, ___1_predicate, method);
}
inline void Iterator_1__ctor_m9788F4EF50A0094F325E620BFC1D32ED3DAE63BB (Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E* __this, const RuntimeMethod* method)
{
	((  void (*) (Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*, const RuntimeMethod*))Iterator_1__ctor_m9788F4EF50A0094F325E620BFC1D32ED3DAE63BB_gshared)(__this, method);
}
inline void WhereSelectListIterator_2__ctor_mA035EEA0C242B3BEB72DA969A7D95966BC3C3DCF (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F*, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29*, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F*, Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_mA035EEA0C242B3BEB72DA969A7D95966BC3C3DCF_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline int32_t Func_2_Invoke_m168B819DF4F932C2AD05BF95F897984BE8B5B6C0_inline (Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*))Func_2_Invoke_m168B819DF4F932C2AD05BF95F897984BE8B5B6C0_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereEnumerableIterator_1__ctor_m6C929ED5C1A2B964D41834542D001D44C5784AE1 (WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374* __this, RuntimeObject* ___0_source, Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821* ___1_predicate, const RuntimeMethod* method)
{
	((  void (*) (WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374*, RuntimeObject*, Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821*, const RuntimeMethod*))WhereEnumerableIterator_1__ctor_m6C929ED5C1A2B964D41834542D001D44C5784AE1_gshared)(__this, ___0_source, ___1_predicate, method);
}
inline void Iterator_1__ctor_m1B09BFBB8B83E605D7C7487F0E6A4986CE070943 (Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D* __this, const RuntimeMethod* method)
{
	((  void (*) (Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*, const RuntimeMethod*))Iterator_1__ctor_m1B09BFBB8B83E605D7C7487F0E6A4986CE070943_gshared)(__this, method);
}
inline void WhereSelectListIterator_2__ctor_m76DFD3D8B5B62444383A84BE159C61CBE99D98DA (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7*, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29*, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F*, Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_m76DFD3D8B5B62444383A84BE159C61CBE99D98DA_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 Func_2_Invoke_mA098B5996B6AE11EADA0A2F2DE377135468CEBAC_inline (Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method)
{
	return ((  InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 (*) (Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*))Func_2_Invoke_mA098B5996B6AE11EADA0A2F2DE377135468CEBAC_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereEnumerableIterator_1__ctor_m0C64291A42967A645028F01B6069873BFB0831E4 (WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C* __this, RuntimeObject* ___0_source, Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA* ___1_predicate, const RuntimeMethod* method)
{
	((  void (*) (WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C*, RuntimeObject*, Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA*, const RuntimeMethod*))WhereEnumerableIterator_1__ctor_m0C64291A42967A645028F01B6069873BFB0831E4_gshared)(__this, ___0_source, ___1_predicate, method);
}
inline void Iterator_1__ctor_m3E47867714E05673E54C6B73D9242F5FFADA1F63 (Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA* __this, const RuntimeMethod* method)
{
	((  void (*) (Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*, const RuntimeMethod*))Iterator_1__ctor_m3E47867714E05673E54C6B73D9242F5FFADA1F63_gshared)(__this, method);
}
inline void WhereSelectListIterator_2__ctor_mDEC1BA11C8B63FF5BC0568D2F273C66867A0FC83 (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC*, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29*, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F*, Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_mDEC1BA11C8B63FF5BC0568D2F273C66867A0FC83_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline RuntimeObject* Func_2_Invoke_m3C9BCAF1F1F54BB85749F5001D1DFDBEAB600AC8_inline (Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method)
{
	return ((  RuntimeObject* (*) (Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*))Func_2_Invoke_m3C9BCAF1F1F54BB85749F5001D1DFDBEAB600AC8_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereEnumerableIterator_1__ctor_m8C0DA4CDA5431C03561F67C4393BB18CDD891F01 (WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4* __this, RuntimeObject* ___0_source, Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00* ___1_predicate, const RuntimeMethod* method)
{
	((  void (*) (WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4*, RuntimeObject*, Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00*, const RuntimeMethod*))WhereEnumerableIterator_1__ctor_m8C0DA4CDA5431C03561F67C4393BB18CDD891F01_gshared)(__this, ___0_source, ___1_predicate, method);
}
inline void Iterator_1__ctor_m9CBF3685A44F015ED364E012FC67ACDC7935DD96 (Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5* __this, const RuntimeMethod* method)
{
	((  void (*) (Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*, const RuntimeMethod*))Iterator_1__ctor_m9CBF3685A44F015ED364E012FC67ACDC7935DD96_gshared)(__this, method);
}
inline void WhereSelectListIterator_2__ctor_mF1B366622F25F80FCC3B4A235B88A3E1F8D3C81B (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9*, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A*, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_mF1B366622F25F80FCC3B4A235B88A3E1F8D3C81B_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13 (List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* __this, const RuntimeMethod* method)
{
	return ((  Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB (*) (List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A*, const RuntimeMethod*))List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13_gshared)(__this, method);
}
inline JsonValue_t01DB320267C848E729A400EF2345979978F851D2 Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_inline (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* __this, const RuntimeMethod* method)
{
	return ((  JsonValue_t01DB320267C848E729A400EF2345979978F851D2 (*) (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*, const RuntimeMethod*))Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_gshared_inline)(__this, method);
}
inline bool Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline (Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method)
{
	return ((  bool (*) (Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*))Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_gshared_inline)(__this, ___0_arg, method);
}
inline bool Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* __this, const RuntimeMethod* method)
{
	return ((  bool (*) (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*, const RuntimeMethod*))Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF_gshared)(__this, method);
}
inline void WhereEnumerableIterator_1__ctor_m2672E4EFA1C94475E8D626D4793081BAA021C25C (WhereEnumerableIterator_1_tC79E61165AF53521C305E751B0FFF9FDBE427D6D* __this, RuntimeObject* ___0_source, Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB* ___1_predicate, const RuntimeMethod* method)
{
	((  void (*) (WhereEnumerableIterator_1_tC79E61165AF53521C305E751B0FFF9FDBE427D6D*, RuntimeObject*, Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB*, const RuntimeMethod*))WhereEnumerableIterator_1__ctor_m2672E4EFA1C94475E8D626D4793081BAA021C25C_gshared)(__this, ___0_source, ___1_predicate, method);
}
inline void WhereSelectListIterator_2__ctor_m9D9A35DD876EEDD07A424ECC1BF4F949472E9B4C (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5*, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A*, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_m9D9A35DD876EEDD07A424ECC1BF4F949472E9B4C_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA Func_2_Invoke_m5FCD56659798D9D7163D543B4EFB36176A166CC2_inline (Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method)
{
	return ((  InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA (*) (Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*))Func_2_Invoke_m5FCD56659798D9D7163D543B4EFB36176A166CC2_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereSelectListIterator_2__ctor_m2F45E03F13987C106503E1A03492C57DFF49FA6D (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730*, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A*, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_m2F45E03F13987C106503E1A03492C57DFF49FA6D_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline int32_t Func_2_Invoke_m321B177CDE142D1D55BBD1D63153186A54FEE249_inline (Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*))Func_2_Invoke_m321B177CDE142D1D55BBD1D63153186A54FEE249_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereSelectListIterator_2__ctor_mC28B3C62105E92A56FF8F653549CD0EAF52EE6F7 (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9*, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A*, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_mC28B3C62105E92A56FF8F653549CD0EAF52EE6F7_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline int32_t Func_2_Invoke_m42E5D81BF1869A4411FD8979982060200B5C8644_inline (Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*))Func_2_Invoke_m42E5D81BF1869A4411FD8979982060200B5C8644_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereSelectListIterator_2__ctor_mA16961886A67BC49FFE65D6057D3BA28D407DF89 (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E*, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A*, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_mA16961886A67BC49FFE65D6057D3BA28D407DF89_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 Func_2_Invoke_m7C8D770BA29067A536942979753FAB53ED84A348_inline (Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method)
{
	return ((  InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 (*) (Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*))Func_2_Invoke_m7C8D770BA29067A536942979753FAB53ED84A348_gshared_inline)(__this, ___0_arg, method);
}
inline void WhereSelectListIterator_2__ctor_mDB747029572F65F32894254A0E779C445B0B1E15 (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* ___2_selector, const RuntimeMethod* method)
{
	((  void (*) (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F*, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A*, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169*, Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0*, const RuntimeMethod*))WhereSelectListIterator_2__ctor_mDB747029572F65F32894254A0E779C445B0B1E15_gshared)(__this, ___0_source, ___1_predicate, ___2_selector, method);
}
inline RuntimeObject* Func_2_Invoke_mCF21D6903C1C9AB285FB0E244CDE0E9ACAC429ED_inline (Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method)
{
	return ((  RuntimeObject* (*) (Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*))Func_2_Invoke_mCF21D6903C1C9AB285FB0E244CDE0E9ACAC429ED_gshared_inline)(__this, ___0_arg, method);
}
inline WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* JsonConvert_DeserializeIntoObject_TisWitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507_m0FD2F6631293E41A0DAD13F38DD6D011B88E37B0 (WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* ___0_instance, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___1_jsonToken, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA* ___2_customConverters, bool ___3_suppressWarnings, const RuntimeMethod* method)
{
	return ((  WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* (*) (WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507*, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C*, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*, bool, const RuntimeMethod*))JsonConvert_DeserializeIntoObject_TisRuntimeObject_m88B0EA93F53DC9CBB88A3E31C7D4648EC392CF9E_gshared)(___0_instance, ___1_jsonToken, ___2_customConverters, ___3_suppressWarnings, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Int32_ToString_m030E01C24E294D6762FB0B6F37CB541581F55CA5 (int32_t* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2 (RuntimeObject* __this, const RuntimeMethod* method) ;
inline WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* JsonConvert_DeserializeIntoObject_TisWitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A_m98EB4FF531C614B59CCD6BC277153895BDF2EA48 (WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* ___0_instance, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___1_jsonToken, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA* ___2_customConverters, bool ___3_suppressWarnings, const RuntimeMethod* method)
{
	return ((  WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* (*) (WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A*, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C*, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*, bool, const RuntimeMethod*))JsonConvert_DeserializeIntoObject_TisRuntimeObject_m88B0EA93F53DC9CBB88A3E31C7D4648EC392CF9E_gshared)(___0_instance, ___1_jsonToken, ___2_customConverters, ___3_suppressWarnings, method);
}
inline WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* JsonConvert_DeserializeIntoObject_TisWitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960_mB99E5FCA35B6DB26C561A8A56D5B255D3097D113 (WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* ___0_instance, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___1_jsonToken, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA* ___2_customConverters, bool ___3_suppressWarnings, const RuntimeMethod* method)
{
	return ((  WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* (*) (WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960*, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C*, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*, bool, const RuntimeMethod*))JsonConvert_DeserializeIntoObject_TisRuntimeObject_m88B0EA93F53DC9CBB88A3E31C7D4648EC392CF9E_gshared)(___0_instance, ___1_jsonToken, ___2_customConverters, ___3_suppressWarnings, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Single_ToString_mE282EDA9CA4F7DF88432D807732837A629D04972 (float* __this, const RuntimeMethod* method) ;
inline void WorkSlice_1__ctor_mF7CEAE61925236C3A0BE9E92E9B97B65EF5DFFEF (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD*, int32_t, int32_t, const RuntimeMethod*))WorkSlice_1__ctor_mF7CEAE61925236C3A0BE9E92E9B97B65EF5DFFEF_gshared)(__this, ___0_src, ___1_srcStart, ___2_srcLen, method);
}
inline void WorkSlice_1__ctor_m8B26C5F2B3AB3C05CB4ACDE5A17C6559BDEF503C (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD*, int32_t, const RuntimeMethod*))WorkSlice_1__ctor_m8B26C5F2B3AB3C05CB4ACDE5A17C6559BDEF503C_gshared)(__this, ___0_src, ___1_srcLen, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Math_Min_m53C488772A34D53917BCA2A491E79A0A5356ED52 (int32_t ___0_val1, int32_t ___1_val2, const RuntimeMethod* method) ;
inline Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 WorkSlice_1_get_Item_m31D876CE45C89B17D24E2D2EB6534C4150D72EE5 (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	return ((  Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 (*) (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*, int32_t, const RuntimeMethod*))WorkSlice_1_get_Item_m31D876CE45C89B17D24E2D2EB6534C4150D72EE5_gshared)(__this, ___0_index, method);
}
inline void WorkSlice_1_set_Item_m242025FA56F79603361582298B70AB2284A12067 (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, int32_t ___0_index, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___1_value, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*, int32_t, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3, const RuntimeMethod*))WorkSlice_1_set_Item_m242025FA56F79603361582298B70AB2284A12067_gshared)(__this, ___0_index, ___1_value, method);
}
inline int32_t WorkSlice_1_get_length_mCDE5BAF472BC1BEBC9D091F532AC1A07D65DB0BC_inline (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*, const RuntimeMethod*))WorkSlice_1_get_length_mCDE5BAF472BC1BEBC9D091F532AC1A07D65DB0BC_gshared_inline)(__this, method);
}
inline int32_t WorkSlice_1_get_capacity_m514E3E1482974088A807521F9E2C481EB83C25F8 (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*, const RuntimeMethod*))WorkSlice_1_get_capacity_m514E3E1482974088A807521F9E2C481EB83C25F8_gshared)(__this, method);
}
inline void Sorting_QuickSort_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mB8556CFA6B9237741AAA1ADF2EE68AA3696F9477 (Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_data, int32_t ___1_start, int32_t ___2_end, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F* ___3_compare, const RuntimeMethod* method)
{
	((  void (*) (Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD*, int32_t, int32_t, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F*, const RuntimeMethod*))Sorting_QuickSort_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mB8556CFA6B9237741AAA1ADF2EE68AA3696F9477_gshared)(___0_data, ___1_start, ___2_end, ___3_compare, method);
}
inline void WorkSlice_1_Sort_m9DF994BABD5BF00DE8FEB14D82F7A0A9F6CAEE3D (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F* ___0_compare, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F*, const RuntimeMethod*))WorkSlice_1_Sort_m9DF994BABD5BF00DE8FEB14D82F7A0A9F6CAEE3D_gshared)(__this, ___0_compare, method);
}
inline void WorkSlice_1__ctor_mFEB81358558CEF0264CCE077535DB880EA2492BA (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*, int32_t, const RuntimeMethod*))WorkSlice_1__ctor_mFEB81358558CEF0264CCE077535DB880EA2492BA_gshared)(__this, ___0_src, ___1_srcLen, method);
}
inline void WorkSlice_1__ctor_m7AC3CC7AABC83B76D23D2B94329DD4D0200156FA (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*, int32_t, int32_t, const RuntimeMethod*))WorkSlice_1__ctor_m7AC3CC7AABC83B76D23D2B94329DD4D0200156FA_gshared)(__this, ___0_src, ___1_srcStart, ___2_srcLen, method);
}
inline void WorkSlice_1_get_Item_mFDEC427E08156ECD6879AD45AEE3618B43E3F726 (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, int32_t ___0_index, Il2CppFullySharedGenericAny* il2cppRetVal, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, int32_t, Il2CppFullySharedGenericAny*, const RuntimeMethod*))WorkSlice_1_get_Item_mFDEC427E08156ECD6879AD45AEE3618B43E3F726_gshared)((WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*)__this, ___0_index, il2cppRetVal, method);
}
inline void WorkSlice_1_set_Item_m757F8BE76FAE27C149DE7C474A2B1845E08A5A0F (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, int32_t ___0_index, Il2CppFullySharedGenericAny ___1_value, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, int32_t, Il2CppFullySharedGenericAny, const RuntimeMethod*))WorkSlice_1_set_Item_m757F8BE76FAE27C149DE7C474A2B1845E08A5A0F_gshared)((WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*)__this, ___0_index, ___1_value, method);
}
inline int32_t WorkSlice_1_get_length_m7212817506EACBA1AB0D914DE401C6FA05F0DD9D_inline (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, const RuntimeMethod*))WorkSlice_1_get_length_m7212817506EACBA1AB0D914DE401C6FA05F0DD9D_gshared_inline)(__this, method);
}
inline int32_t WorkSlice_1_get_capacity_mCF0D603B7EC6E6037D0E1A14D8D0F49AD063E59E (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, const RuntimeMethod*))WorkSlice_1_get_capacity_mCF0D603B7EC6E6037D0E1A14D8D0F49AD063E59E_gshared)(__this, method);
}
inline void WorkSlice_1_Sort_mE2ED392BDF8F4F4141D7BF4C984EFE943F607A94 (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9* ___0_compare, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9*, const RuntimeMethod*))WorkSlice_1_Sort_mE2ED392BDF8F4F4141D7BF4C984EFE943F607A94_gshared)(__this, ___0_compare, method);
}
inline void WorkSlice_1__ctor_mC3BCAA8301A4E37DDB26811AAFFE1A3654FA47D0 (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263*, int32_t, int32_t, const RuntimeMethod*))WorkSlice_1__ctor_mC3BCAA8301A4E37DDB26811AAFFE1A3654FA47D0_gshared)(__this, ___0_src, ___1_srcStart, ___2_srcLen, method);
}
inline void WorkSlice_1__ctor_m76CCC65E3DFB8B84A2154B65A79B56688F9B26A4 (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263*, int32_t, const RuntimeMethod*))WorkSlice_1__ctor_m76CCC65E3DFB8B84A2154B65A79B56688F9B26A4_gshared)(__this, ___0_src, ___1_srcLen, method);
}
inline LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 WorkSlice_1_get_Item_mFD8EE50B88077F3EF9BCEF97BD6D0352D2E8445D (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	return ((  LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 (*) (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*, int32_t, const RuntimeMethod*))WorkSlice_1_get_Item_mFD8EE50B88077F3EF9BCEF97BD6D0352D2E8445D_gshared)(__this, ___0_index, method);
}
inline void WorkSlice_1_set_Item_m070889CA2F62E82384BAB3CEF6D6E9AABF153663 (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, int32_t ___0_index, LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 ___1_value, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*, int32_t, LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2, const RuntimeMethod*))WorkSlice_1_set_Item_m070889CA2F62E82384BAB3CEF6D6E9AABF153663_gshared)(__this, ___0_index, ___1_value, method);
}
inline int32_t WorkSlice_1_get_length_m0E5769CB5B052657E7327DCAD0F2CA104327D7D8_inline (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*, const RuntimeMethod*))WorkSlice_1_get_length_m0E5769CB5B052657E7327DCAD0F2CA104327D7D8_gshared_inline)(__this, method);
}
inline int32_t WorkSlice_1_get_capacity_m98A237D126494A11FF3C61211B36A790E4E8A3B0 (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*, const RuntimeMethod*))WorkSlice_1_get_capacity_m98A237D126494A11FF3C61211B36A790E4E8A3B0_gshared)(__this, method);
}
inline void Sorting_QuickSort_TisLightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2_mD7019ED48D20810C1169430283118F96945E9450 (LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_data, int32_t ___1_start, int32_t ___2_end, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___3_compare, const RuntimeMethod* method)
{
	((  void (*) (LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263*, int32_t, int32_t, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821*, const RuntimeMethod*))Sorting_QuickSort_TisLightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2_mD7019ED48D20810C1169430283118F96945E9450_gshared)(___0_data, ___1_start, ___2_end, ___3_compare, method);
}
inline void WorkSlice_1_Sort_m7EF532E936D55845DAAC606C0A214FE48EBF8584 (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___0_compare, const RuntimeMethod* method)
{
	((  void (*) (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821*, const RuntimeMethod*))WorkSlice_1_Sort_m7EF532E936D55845DAAC606C0A214FE48EBF8584_gshared)(__this, ___0_compare, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Environment_get_CurrentManagedThreadId_m66483AADCCC13272EBDCD94D31D2E52603C24BDF (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA (RuntimeObject* ___0_obj, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF (int32_t* ___0_location1, int32_t ___1_value, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149 (RuntimeObject* ___0_obj, bool* ___1_lockTaken, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Array_Copy_mB4904E17BD92E320613A3251C0205E0786B3BF41 (RuntimeArray* ___0_sourceArray, int32_t ___1_sourceIndex, RuntimeArray* ___2_destinationArray, int32_t ___3_destinationIndex, int32_t ___4_length, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int64_t Interlocked_Increment_m12CD643CCD5FCDE95DBC0AB07D09AF696E26F9C0 (int64_t* ___0_location, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Array_Clear_m50BAA3751899858B097D3FF2ED31F284703FE5CB (RuntimeArray* ___0_array, int32_t ___1_index, int32_t ___2_length, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void SpinWait_SpinOnce_m5B74E6B15013E90667646C0D943E886D4EC596AF (SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675* __this, const RuntimeMethod* method) ;
inline int32_t WorkStealingQueue_get_DangerousCount_m7EA23F3971CF379A3C0A87B49FECB670CD7D738E (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896*, const RuntimeMethod*))WorkStealingQueue_get_DangerousCount_m7EA23F3971CF379A3C0A87B49FECB670CD7D738E_gshared)(__this, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717 (int32_t* ___0_location1, int32_t ___1_value, int32_t ___2_comparand, const RuntimeMethod* method) ;
inline String_t* ExtractKeyDelegate_Invoke_m3214A3F92C2EB22088AB7738E8DFE1BE70925148_inline (ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* __this, RuntimeObject* ___0_value, const RuntimeMethod* method)
{
	return ((  String_t* (*) (ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583*, RuntimeObject*, const RuntimeMethod*))ExtractKeyDelegate_Invoke_m3214A3F92C2EB22088AB7738E8DFE1BE70925148_gshared_inline)(__this, ___0_value, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void OverflowException__ctor_m7F6A928C9BE47384586BDDE8B4B87666421E0F1A (OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C* __this, const RuntimeMethod* method) ;
inline void XHashtableState__ctor_m37E975C296D7D8CDF9ACAA9ABD7C4A0C82C427D3 (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* ___0_extractKey, int32_t ___1_capacity, const RuntimeMethod* method)
{
	((  void (*) (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A*, ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583*, int32_t, const RuntimeMethod*))XHashtableState__ctor_m37E975C296D7D8CDF9ACAA9ABD7C4A0C82C427D3_gshared)(__this, ___0_extractKey, ___1_capacity, method);
}
inline bool XHashtableState_TryAdd_m951A9A914307495745E2B33DB0BA6583FB4060A1 (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, RuntimeObject* ___0_value, RuntimeObject** ___1_newValue, const RuntimeMethod* method)
{
	return ((  bool (*) (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A*, RuntimeObject*, RuntimeObject**, const RuntimeMethod*))XHashtableState_TryAdd_m951A9A914307495745E2B33DB0BA6583FB4060A1_gshared)(__this, ___0_value, ___1_newValue, method);
}
inline int32_t XHashtableState_ComputeHashCode_mD58A823B4A09F8DB93853EA9536E6B457B59F1B1 (String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, const RuntimeMethod* method)
{
	return ((  int32_t (*) (String_t*, int32_t, int32_t, const RuntimeMethod*))XHashtableState_ComputeHashCode_mD58A823B4A09F8DB93853EA9536E6B457B59F1B1_gshared)(___0_key, ___1_index, ___2_count, method);
}
inline bool XHashtableState_FindEntry_m3AC0430564190D3FCAD9A8DB1DD0E725F7392A91 (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, int32_t ___0_hashCode, String_t* ___1_key, int32_t ___2_index, int32_t ___3_count, int32_t* ___4_entryIndex, const RuntimeMethod* method)
{
	return ((  bool (*) (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A*, int32_t, String_t*, int32_t, int32_t, int32_t*, const RuntimeMethod*))XHashtableState_FindEntry_m3AC0430564190D3FCAD9A8DB1DD0E725F7392A91_gshared)(__this, ___0_hashCode, ___1_key, ___2_index, ___3_count, ___4_entryIndex, method);
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline (String_t* __this, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Interlocked_Increment_m3C240C32E8D9544EC050B74D4F28EEB58F1F9309 (int32_t* ___0_location, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Thread_MemoryBarrier_m83873F1E6CEB16C0781941141382DA874A36097D (const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t String_CompareOrdinal_m8940CFAE90021ED8DA3F2DF8226941C9EEB2E32D (String_t* ___0_strA, int32_t ___1_indexA, String_t* ___2_strB, int32_t ___3_indexB, int32_t ___4_length, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Il2CppChar String_get_Chars_mC49DF0CD2D3BE7BE97B3AD9C995BE3094F8E36D3 (String_t* __this, int32_t ___0_index, const RuntimeMethod* method) ;
inline bool XHashtableState_TryGetValue_m22BFB31B1A4E2CEDE4CA7133F4A0B50B9E514308 (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, RuntimeObject** ___3_value, const RuntimeMethod* method)
{
	return ((  bool (*) (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A*, String_t*, int32_t, int32_t, RuntimeObject**, const RuntimeMethod*))XHashtableState_TryGetValue_m22BFB31B1A4E2CEDE4CA7133F4A0B50B9E514308_gshared)(__this, ___0_key, ___1_index, ___2_count, ___3_value, method);
}
inline XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* XHashtableState_Resize_mDB3BA5C327EF6A86F6CE58B59E1D7430E62532C5 (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, const RuntimeMethod* method)
{
	return ((  XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* (*) (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A*, const RuntimeMethod*))XHashtableState_Resize_mDB3BA5C327EF6A86F6CE58B59E1D7430E62532C5_gshared)(__this, method);
}
inline List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_inline (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method)
{
	return ((  List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* (*) (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339*, const RuntimeMethod*))YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_gshared_inline)(__this, method);
}
inline int32_t List_1_get_Count_m4407E4C389F22B8CEC282C15D56516658746C383_inline (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D*, const RuntimeMethod*))List_1_get_Count_m4407E4C389F22B8CEC282C15D56516658746C383_gshared_inline)(__this, method);
}
inline RuntimeObject* List_1_get_Item_m33561245D64798C2AB07584C0EC4F240E4839A38 (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	return ((  RuntimeObject* (*) (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D*, int32_t, const RuntimeMethod*))List_1_get_Item_m33561245D64798C2AB07584C0EC4F240E4839A38_gshared)(__this, ___0_index, method);
}
inline void Action_1__ctor_m2E1DFA67718FC1A0B6E5DFEB78831FFE9C059EB4 (Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87*, RuntimeObject*, intptr_t, const RuntimeMethod*))Action_1__ctor_m2E1DFA67718FC1A0B6E5DFEB78831FFE9C059EB4_gshared)(__this, ___0_object, ___1_method, method);
}
inline Delegate_t* EventBetter_RegisterInternal_TisYieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339_TisRuntimeObject_mF3B26B943430E9D36CA501D294C18219A17C9D77 (EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3* __this, YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* ___0_listener, Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87* ___1_handler, int32_t ___2_flags, const RuntimeMethod* method)
{
	return ((  Delegate_t* (*) (EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3*, YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339*, Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87*, int32_t, const RuntimeMethod*))EventBetter_RegisterInternal_TisRuntimeObject_TisRuntimeObject_m0C8C9474A4E38D813E4B1BC8B7A8F65A2ED7172D_gshared)(__this, ___0_listener, ___1_handler, ___2_flags, method);
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Type_t* Type_GetTypeFromHandle_m6062B81682F79A4D6DF2640692EE6D9987858C57 (RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B ___0_handle, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool EventBetter_UnlistenHandler_mCFBA2C99EAD4AF0856CC60FA89B7A7C698EE5EC4 (EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3* __this, Type_t* ___0_messageType, Delegate_t* ___1_handler, const RuntimeMethod* method) ;
inline void List_1__ctor_m7F078BB342729BDF11327FD89D7872265328F690 (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, const RuntimeMethod* method)
{
	((  void (*) (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D*, const RuntimeMethod*))List_1__ctor_m7F078BB342729BDF11327FD89D7872265328F690_gshared)(__this, method);
}
inline void YieldListener_1_set_Messages_mD81C51A83A5A04A77DB20F2FA7BA057016AFCC66_inline (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339*, List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D*, const RuntimeMethod*))YieldListener_1_set_Messages_mD81C51A83A5A04A77DB20F2FA7BA057016AFCC66_gshared_inline)(__this, ___0_value, method);
}
inline void List_1_Add_mEBCF994CC3814631017F46A387B1A192ED6C85C7_inline (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, RuntimeObject* ___0_item, const RuntimeMethod* method)
{
	((  void (*) (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D*, RuntimeObject*, const RuntimeMethod*))List_1_Add_mEBCF994CC3814631017F46A387B1A192ED6C85C7_gshared_inline)(__this, ___0_item, method);
}
inline void YieldListener_1_Dispose_m9C4194F200BBADB8E122CBD8749141F82A3DE8F2 (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method)
{
	((  void (*) (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339*, const RuntimeMethod*))YieldListener_1_Dispose_m9C4194F200BBADB8E122CBD8749141F82A3DE8F2_gshared)(__this, method);
}
inline void YieldListener_1_OnMessage_mF071E8A97B1E58D5F5007B04FD9008F26196EEC7 (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, RuntimeObject* ___0_msg, const RuntimeMethod* method)
{
	((  void (*) (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339*, RuntimeObject*, const RuntimeMethod*))YieldListener_1_OnMessage_mF071E8A97B1E58D5F5007B04FD9008F26196EEC7_gshared)(__this, ___0_msg, method);
}
inline void List_1_AddWithResize_m79A9BF770BEF9C06BE40D5401E55E375F2726CC4 (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, RuntimeObject* ___0_item, const RuntimeMethod* method)
{
	((  void (*) (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D*, RuntimeObject*, const RuntimeMethod*))List_1_AddWithResize_m79A9BF770BEF9C06BE40D5401E55E375F2726CC4_gshared)(__this, ___0_item, method);
}
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m74371F82A28BD893528CD04F939FEBF8DBAB8208_gshared (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_mB4FD6E6C2748201577FA407A6347A551413E8DA0((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610* WhereSelectListIterator_2_Clone_m91E1FBBE0EC380FE80207BEF0FFCE08066217921_gshared (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736* __this, const RuntimeMethod* method) 
{
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = __this->___source;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = __this->___predicate;
		Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* L_2 = __this->___selector;
		WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736* L_3 = (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_m74371F82A28BD893528CD04F939FEBF8DBAB8208(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_m22B350EEFC34B3A03EBC73AA3544344B6DDE4901_gshared (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 L_4;
		L_4 = List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___m_String), (void*)NULL);
		#endif
		((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_5 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_6;
		L_6 = Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_8 = __this->___predicate;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* L_11 = __this->___selector;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_12 = V_1;
		NullCheck(L_11);
		InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA L_13;
		L_13 = Func_2_Invoke_mD2BF1A07861A43FD1AC9DAAB47815991AD1A959F_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___current = L_13;
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___current))->___U3CInstanceU3Ek__BackingField), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___current))->___U3CTypeU3Ek__BackingField), (void*)NULL);
		#endif
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_14 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_m89777149F2ADD9AF28AD9534FEA4FB50ED4947DD_gshared (WhereSelectListIterator_2_t516B2672ADD72F78C0900473EA1AEA82CA87F736* __this, Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9* L_1 = (WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_mCD2AE447BF5F2DB9BD2FCC12898977FE4F004AB7(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mE284E07CB05A30CB3CD67A6B975FFBB56EA2B829_gshared (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m426D8BD95B00FDA5C5E8FAE956FCB74E54BC7CC7((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07* WhereSelectListIterator_2_Clone_mAA6E54E622F7BE663D868824A8964114883CF9B8_gshared (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484* __this, const RuntimeMethod* method) 
{
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = __this->___source;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = __this->___predicate;
		Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* L_2 = __this->___selector;
		WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484* L_3 = (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_mE284E07CB05A30CB3CD67A6B975FFBB56EA2B829(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_m3C3E603A5CAAA0B3816226A84D4D8695B2A8033B_gshared (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 L_4;
		L_4 = List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___m_String), (void*)NULL);
		#endif
		((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_5 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_6;
		L_6 = Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_8 = __this->___predicate;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* L_11 = __this->___selector;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_12 = V_1;
		NullCheck(L_11);
		int32_t L_13;
		L_13 = Func_2_Invoke_mF4550AAF07822B89D9CEDD14EA0A726DC11B8487_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this)->___current = L_13;
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_14 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_mA5DFDE58967126D391822B5CCAEAD14051DD8CF3_gshared (WhereSelectListIterator_2_t793E1457EA65F982F1E56BAD6F9D0C97047DF484* __this, Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A* L_1 = (WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_mA7F4C6508A043D99B8288E43867D576F4697E4EE(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mA035EEA0C242B3BEB72DA969A7D95966BC3C3DCF_gshared (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m9788F4EF50A0094F325E620BFC1D32ED3DAE63BB((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E* WhereSelectListIterator_2_Clone_m509716DB871BE511DEC69403088B36536CEE5B38_gshared (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F* __this, const RuntimeMethod* method) 
{
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = __this->___source;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = __this->___predicate;
		Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* L_2 = __this->___selector;
		WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F* L_3 = (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_mA035EEA0C242B3BEB72DA969A7D95966BC3C3DCF(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_m5EB1C83CE1ED75B5B35A743192C5F010E88A95B7_gshared (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 L_4;
		L_4 = List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___m_String), (void*)NULL);
		#endif
		((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_5 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_6;
		L_6 = Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_8 = __this->___predicate;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* L_11 = __this->___selector;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_12 = V_1;
		NullCheck(L_11);
		int32_t L_13;
		L_13 = Func_2_Invoke_m168B819DF4F932C2AD05BF95F897984BE8B5B6C0_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this)->___current = L_13;
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_14 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_m67B440EF495CA2C47DA05925057CFB4093DB5DD1_gshared (WhereSelectListIterator_2_t68E85FC2E8FD7AE002007B6A88A78C7D994A851F* __this, Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374* L_1 = (WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_m6C929ED5C1A2B964D41834542D001D44C5784AE1(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m76DFD3D8B5B62444383A84BE159C61CBE99D98DA_gshared (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m1B09BFBB8B83E605D7C7487F0E6A4986CE070943((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D* WhereSelectListIterator_2_Clone_m7514B4D4CA43073C8D6D3B10D6B33C562464A0C8_gshared (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7* __this, const RuntimeMethod* method) 
{
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = __this->___source;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = __this->___predicate;
		Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* L_2 = __this->___selector;
		WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7* L_3 = (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_m76DFD3D8B5B62444383A84BE159C61CBE99D98DA(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_m9801C3269DA1FCC381EED100CE2C710B10873FDE_gshared (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 L_4;
		L_4 = List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___m_String), (void*)NULL);
		#endif
		((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_5 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_6;
		L_6 = Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_8 = __this->___predicate;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* L_11 = __this->___selector;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_12 = V_1;
		NullCheck(L_11);
		InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 L_13;
		L_13 = Func_2_Invoke_mA098B5996B6AE11EADA0A2F2DE377135468CEBAC_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___current = L_13;
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___current))->___m_StringOriginalCase), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___current))->___m_StringLowerCase), (void*)NULL);
		#endif
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_14 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_m54A65CB2977BDF55DA3004F81CF889F22E233DE9_gshared (WhereSelectListIterator_2_t783076280AA5A0279F1CF30B5F192A923D62C1E7* __this, Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C* L_1 = (WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_m0C64291A42967A645028F01B6069873BFB0831E4(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mDEC1BA11C8B63FF5BC0568D2F273C66867A0FC83_gshared (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC* __this, List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* ___0_source, Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* ___1_predicate, Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m3E47867714E05673E54C6B73D9242F5FFADA1F63((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA* WhereSelectListIterator_2_Clone_m7F15BA24C1AD78593E0EFF5AA8220436BDB66612_gshared (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC* __this, const RuntimeMethod* method) 
{
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_0 = __this->___source;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_1 = __this->___predicate;
		Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* L_2 = __this->___selector;
		WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC* L_3 = (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_mDEC1BA11C8B63FF5BC0568D2F273C66867A0FC83(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_m56890DBD7344EE51AE2457C222E70BE9AF955C00_gshared (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	Substring_t2E16755269E6716C22074D6BC0A9099915E67849 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_tC4C8D746916C433D3343B92925052F1B9DB34A29* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785 L_4;
		L_4 = List_1_GetEnumerator_m2E8AAA332A1CCE110C46806675D27756C848C264(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___m_String), (void*)NULL);
		#endif
		((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_5 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_6;
		L_6 = Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* L_8 = __this->___predicate;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* L_11 = __this->___selector;
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_12 = V_1;
		NullCheck(L_11);
		RuntimeObject* L_13;
		L_13 = Func_2_Invoke_m3C9BCAF1F1F54BB85749F5001D1DFDBEAB600AC8_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___current = L_13;
		Il2CppCodeGenWriteBarrier((void**)(&((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___current), (void*)L_13);
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* L_14 = (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_mBD8837024473F97D1F793AD3DF5E27568D7BDD06(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_m873C15FE36A52528B396D6A1919D572AFDCA52B6_gshared (WhereSelectListIterator_2_t943FB9C430D28BC0B04E2A8E2B3E914860BCA6BC* __this, Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4* L_1 = (WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_m8C0DA4CDA5431C03561F67C4393BB18CDD891F01(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m6BFCBB5460270ED1896D24DC7E3B83F4950D2140_gshared (WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336* __this, List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A* ___0_source, Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B* ___1_predicate, Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0* ___2_selector, const RuntimeMethod* method) 
{
	{
		((  void (*) (Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 0)))((Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A* L_0 = ___0_source;
		il2cpp_codegen_write_instance_field_data<List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A*>(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),0), L_0);
		Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B* L_1 = ___1_predicate;
		il2cpp_codegen_write_instance_field_data<Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B*>(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),1), L_1);
		Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0* L_2 = ___2_selector;
		il2cpp_codegen_write_instance_field_data<Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0*>(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),2), L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0* WhereSelectListIterator_2_Clone_m8EC8E684FFDC3BC579DF37C08993B7F80966639D_gshared (WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336* __this, const RuntimeMethod* method) 
{
	{
		List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A* L_0 = *(List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A**)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),0));
		Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B* L_1 = *(Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B**)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),1));
		Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0* L_2 = *(Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0**)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),2));
		WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336* L_3 = (WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		((  void (*) (WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336*, List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A*, Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B*, Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 6)))(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_mBB81EEF5DFFEBDDB1AC24116FAD1D13505525569_gshared (WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336* __this, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_TSource_tEB7490DB2885922B8C60E28873F5DB811BD9CEB3 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 12));
	const uint32_t SizeOf_Enumerator_t8A622325AF1352D3AB0ECDBB45A0AFB7AF959716 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 9));
	const uint32_t SizeOf_TResult_t11AC9139084FDCB528CAF75FE5166467D3329A05 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 15));
	const Il2CppFullySharedGenericAny L_5 = alloca(SizeOf_TSource_tEB7490DB2885922B8C60E28873F5DB811BD9CEB3);
	const Il2CppFullySharedGenericAny L_8 = L_5;
	const Il2CppFullySharedGenericAny L_11 = L_5;
	const Il2CppFullySharedGenericAny L_12 = alloca(SizeOf_TResult_t11AC9139084FDCB528CAF75FE5166467D3329A05);
	const Enumerator_tF5AC6CD19D283FBD724440520CEE68FE2602F7AF L_4 = alloca(SizeOf_Enumerator_t8A622325AF1352D3AB0ECDBB45A0AFB7AF959716);
	int32_t V_0 = 0;
	Il2CppFullySharedGenericAny V_1 = alloca(SizeOf_TSource_tEB7490DB2885922B8C60E28873F5DB811BD9CEB3);
	memset(V_1, 0, SizeOf_TSource_tEB7490DB2885922B8C60E28873F5DB811BD9CEB3);
	{
		int32_t L_0 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 7),1));
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A* L_3 = *(List_1_tDBA89B0E21BAC58CFBD3C1F76E4668E3B562761A**)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),0));
		NullCheck(L_3);
		InvokerActionInvoker1< Enumerator_tF5AC6CD19D283FBD724440520CEE68FE2602F7AF* >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 8)), il2cpp_rgctx_method(method->klass->rgctx_data, 8), L_3, (Enumerator_tF5AC6CD19D283FBD724440520CEE68FE2602F7AF*)L_4);
		il2cpp_codegen_write_instance_field_data(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),3), L_4, SizeOf_Enumerator_t8A622325AF1352D3AB0ECDBB45A0AFB7AF959716);
		il2cpp_codegen_write_instance_field_data<int32_t>(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 7),1), 2);
		goto IL_0061;
	}

IL_002b:
	{
		InvokerActionInvoker1< Il2CppFullySharedGenericAny* >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 10)), il2cpp_rgctx_method(method->klass->rgctx_data, 10), (((Enumerator_tF5AC6CD19D283FBD724440520CEE68FE2602F7AF*)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),3)))), (Il2CppFullySharedGenericAny*)L_5);
		il2cpp_codegen_memcpy(V_1, L_5, SizeOf_TSource_tEB7490DB2885922B8C60E28873F5DB811BD9CEB3);
		Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B* L_6 = *(Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B**)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),1));
		if (!L_6)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B* L_7 = *(Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B**)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),1));
		il2cpp_codegen_memcpy(L_8, V_1, SizeOf_TSource_tEB7490DB2885922B8C60E28873F5DB811BD9CEB3);
		NullCheck(L_7);
		bool L_9;
		L_9 = InvokerFuncInvoker1< bool, Il2CppFullySharedGenericAny >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 13)), il2cpp_rgctx_method(method->klass->rgctx_data, 13), L_7, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 12)) ? L_8: *(void**)L_8));
		if (!L_9)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0* L_10 = *(Func_2_t7F5F5324CE2DDB7001B68FFE29A5D9F907139FB0**)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),2));
		il2cpp_codegen_memcpy(L_11, V_1, SizeOf_TSource_tEB7490DB2885922B8C60E28873F5DB811BD9CEB3);
		NullCheck(L_10);
		InvokerActionInvoker2< Il2CppFullySharedGenericAny, Il2CppFullySharedGenericAny* >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 14)), il2cpp_rgctx_method(method->klass->rgctx_data, 14), L_10, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 12)) ? L_11: *(void**)L_11), (Il2CppFullySharedGenericAny*)L_12);
		il2cpp_codegen_write_instance_field_data(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 7),2), L_12, SizeOf_TResult_t11AC9139084FDCB528CAF75FE5166467D3329A05);
		return (bool)1;
	}

IL_0061:
	{
		bool L_13;
		L_13 = ((  bool (*) (Enumerator_tF5AC6CD19D283FBD724440520CEE68FE2602F7AF*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 16)))((((Enumerator_tF5AC6CD19D283FBD724440520CEE68FE2602F7AF*)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3),3)))), il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_13)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_t0F1D8198E840368AC82131EC1FF03EB76BCE73B0*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_m1739BDD134D3AF5A55DBB06AEE130B0C58E47014_gshared (WhereSelectListIterator_2_t86EE6817E8A1706688C6D82D82C9D44BC99CC336* __this, Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t8B24528558F527941435C4FE1D046216FE4F277B* L_1 = (WhereEnumerableIterator_1_t8B24528558F527941435C4FE1D046216FE4F277B*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		((  void (*) (WhereEnumerableIterator_1_t8B24528558F527941435C4FE1D046216FE4F277B*, RuntimeObject*, Func_2_t19E50C11C3E1F20B5A8FDB85D7DD353B6DFF868B*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 20)))(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mF1B366622F25F80FCC3B4A235B88A3E1F8D3C81B_gshared (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m9CBF3685A44F015ED364E012FC67ACDC7935DD96((Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5* WhereSelectListIterator_2_Clone_m0DD3998C6ECC350EAFF25B7F6F4AE9678E8499A9_gshared (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9* __this, const RuntimeMethod* method) 
{
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = __this->___source;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = __this->___predicate;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_2 = __this->___selector;
		WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9* L_3 = (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_mF1B366622F25F80FCC3B4A235B88A3E1F8D3C81B(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_mF66C2E34D7C5C50FD2931FA46113D26293B3FB61_gshared (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	JsonValue_t01DB320267C848E729A400EF2345979978F851D2 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB L_4;
		L_4 = List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&((&((&(((&__this->___enumerator))->____current))->___stringValue))->___text))->___m_String), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___arrayValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___objectValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___anyValue), (void*)NULL);
		#endif
		((Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_5 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_6;
		L_6 = Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_8 = __this->___predicate;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_11 = __this->___selector;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_12 = V_1;
		NullCheck(L_11);
		bool L_13;
		L_13 = Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*)__this)->___current = L_13;
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_14 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_t71E6A09C6DA7B5665A7AE21BA593F9A1C4CBFEA5*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_mCBC9E09603CD064FD1E5B2D2C3BDBCAE1B0D44F6_gshared (WhereSelectListIterator_2_t1CC31E4847357457397826D4649A16053BBB79D9* __this, Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t66AC14B29DD8B1DDD05693A14E55CF7707C762DB* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_tC79E61165AF53521C305E751B0FFF9FDBE427D6D* L_1 = (WhereEnumerableIterator_1_tC79E61165AF53521C305E751B0FFF9FDBE427D6D*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_m2672E4EFA1C94475E8D626D4793081BAA021C25C(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m9D9A35DD876EEDD07A424ECC1BF4F949472E9B4C_gshared (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_mB4FD6E6C2748201577FA407A6347A551413E8DA0((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610* WhereSelectListIterator_2_Clone_m3338CA2AFA8591238B13489692B78C30D705F3C5_gshared (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5* __this, const RuntimeMethod* method) 
{
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = __this->___source;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = __this->___predicate;
		Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* L_2 = __this->___selector;
		WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5* L_3 = (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_m9D9A35DD876EEDD07A424ECC1BF4F949472E9B4C(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_m11D55B8AEE5504CFF91B53017963CCEB7AEB62A0_gshared (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	JsonValue_t01DB320267C848E729A400EF2345979978F851D2 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB L_4;
		L_4 = List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&((&((&(((&__this->___enumerator))->____current))->___stringValue))->___text))->___m_String), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___arrayValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___objectValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___anyValue), (void*)NULL);
		#endif
		((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_5 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_6;
		L_6 = Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_8 = __this->___predicate;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* L_11 = __this->___selector;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_12 = V_1;
		NullCheck(L_11);
		InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA L_13;
		L_13 = Func_2_Invoke_m5FCD56659798D9D7163D543B4EFB36176A166CC2_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___current = L_13;
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___current))->___U3CInstanceU3Ek__BackingField), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this)->___current))->___U3CTypeU3Ek__BackingField), (void*)NULL);
		#endif
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_14 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tAA145424C2242535CCAB871EC1BD3C16344BB610*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_m926409FDE8C5F2528D29B8EA95928825AF70777D_gshared (WhereSelectListIterator_2_tEC549F2F61E5F4BF2156FB9A9F8CF8029A18DAB5* __this, Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_tD2291AFFE4C7FB74D9166AAA5F17244DB9DFB5A5* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9* L_1 = (WhereEnumerableIterator_1_t69CB3C06B2AC218C8912DD48368CC201862252F9*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_mCD2AE447BF5F2DB9BD2FCC12898977FE4F004AB7(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_m2F45E03F13987C106503E1A03492C57DFF49FA6D_gshared (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m426D8BD95B00FDA5C5E8FAE956FCB74E54BC7CC7((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07* WhereSelectListIterator_2_Clone_m017DDC5605971EF6DC6E63CC46E9C1FF67ECF3C3_gshared (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730* __this, const RuntimeMethod* method) 
{
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = __this->___source;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = __this->___predicate;
		Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* L_2 = __this->___selector;
		WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730* L_3 = (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_m2F45E03F13987C106503E1A03492C57DFF49FA6D(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_mE2745F3BB6FB127C4C57AA2EC540DEAA7A89D622_gshared (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	JsonValue_t01DB320267C848E729A400EF2345979978F851D2 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB L_4;
		L_4 = List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&((&((&(((&__this->___enumerator))->____current))->___stringValue))->___text))->___m_String), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___arrayValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___objectValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___anyValue), (void*)NULL);
		#endif
		((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_5 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_6;
		L_6 = Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_8 = __this->___predicate;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* L_11 = __this->___selector;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_12 = V_1;
		NullCheck(L_11);
		int32_t L_13;
		L_13 = Func_2_Invoke_m321B177CDE142D1D55BBD1D63153186A54FEE249_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this)->___current = L_13;
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_14 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tCCB165DB1184FCEC531345224E55B98DDF04BD07*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_mF06218BFCB481A4B87E64900FBFE9FDABB11A7C3_gshared (WhereSelectListIterator_2_t41A7AE956EBDE8CEE5C27A6E3ECD7CD33F668730* __this, Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t63A057E8762189D8C22BF71360D00C1047680DFA* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A* L_1 = (WhereEnumerableIterator_1_t5DBA717695B8C7E60489C86AEB1FF673F50E293A*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_mA7F4C6508A043D99B8288E43867D576F4697E4EE(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mC28B3C62105E92A56FF8F653549CD0EAF52EE6F7_gshared (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m9788F4EF50A0094F325E620BFC1D32ED3DAE63BB((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E* WhereSelectListIterator_2_Clone_mAD5FC9DADDF08C991003C86EA6DF6EA67923BD08_gshared (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9* __this, const RuntimeMethod* method) 
{
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = __this->___source;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = __this->___predicate;
		Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* L_2 = __this->___selector;
		WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9* L_3 = (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_mC28B3C62105E92A56FF8F653549CD0EAF52EE6F7(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_m27D46C85A4861446D84A190A884E4FCB97AC700B_gshared (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	JsonValue_t01DB320267C848E729A400EF2345979978F851D2 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB L_4;
		L_4 = List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&((&((&(((&__this->___enumerator))->____current))->___stringValue))->___text))->___m_String), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___arrayValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___objectValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___anyValue), (void*)NULL);
		#endif
		((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_5 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_6;
		L_6 = Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_8 = __this->___predicate;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* L_11 = __this->___selector;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_12 = V_1;
		NullCheck(L_11);
		int32_t L_13;
		L_13 = Func_2_Invoke_m42E5D81BF1869A4411FD8979982060200B5C8644_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this)->___current = L_13;
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_14 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tEEF82AE11FFC6DF9B283B2D5E4A87B3E3BFD805E*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_mAD2AF32884598AE600AFBA562BB6C45ADF04234F_gshared (WhereSelectListIterator_2_t5C6FF44C18E22CC6D264FFA3E490BBDA91B1B3E9* __this, Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t1C8F983F9A1AA802D45E89037E2AA7ACD1094821* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374* L_1 = (WhereEnumerableIterator_1_t7F233A4115A2DA7F213075592407137456080374*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_m6C929ED5C1A2B964D41834542D001D44C5784AE1(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mA16961886A67BC49FFE65D6057D3BA28D407DF89_gshared (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m1B09BFBB8B83E605D7C7487F0E6A4986CE070943((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D* WhereSelectListIterator_2_Clone_m01258FBF3047EACEBBBE89855BDAE5197B095A79_gshared (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E* __this, const RuntimeMethod* method) 
{
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = __this->___source;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = __this->___predicate;
		Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* L_2 = __this->___selector;
		WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E* L_3 = (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_mA16961886A67BC49FFE65D6057D3BA28D407DF89(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_mE119EF6FA88D7E63491FC5611A5F34D3602F5D1B_gshared (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	JsonValue_t01DB320267C848E729A400EF2345979978F851D2 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB L_4;
		L_4 = List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&((&((&(((&__this->___enumerator))->____current))->___stringValue))->___text))->___m_String), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___arrayValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___objectValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___anyValue), (void*)NULL);
		#endif
		((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_5 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_6;
		L_6 = Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_8 = __this->___predicate;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* L_11 = __this->___selector;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_12 = V_1;
		NullCheck(L_11);
		InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 L_13;
		L_13 = Func_2_Invoke_m7C8D770BA29067A536942979753FAB53ED84A348_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___current = L_13;
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___current))->___m_StringOriginalCase), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&(((&((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this)->___current))->___m_StringLowerCase), (void*)NULL);
		#endif
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_14 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_tE6B605B8CEAAA7680455D82B5BF52914D0C3892D*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_mAEEE2C3DA2DBA62F84C02FE5EDD8894338DC5D55_gshared (WhereSelectListIterator_2_t6C3621A89FA834F0251E7EF9F3484147BB4FD39E* __this, Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_t3E602B1155E686D3D1F6672128944CD3D6D9B6FA* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C* L_1 = (WhereEnumerableIterator_1_t98114935A3AA4F0199B53A5027B776D5AFF9AF6C*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_m0C64291A42967A645028F01B6069873BFB0831E4(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 100250
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WhereSelectListIterator_2__ctor_mDB747029572F65F32894254A0E779C445B0B1E15_gshared (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F* __this, List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* ___0_source, Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* ___1_predicate, Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* ___2_selector, const RuntimeMethod* method) 
{
	{
		Iterator_1__ctor_m3E47867714E05673E54C6B73D9242F5FFADA1F63((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this, il2cpp_rgctx_method(method->klass->rgctx_data, 0));
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = ___0_source;
		__this->___source = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___source), (void*)L_0);
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = ___1_predicate;
		__this->___predicate = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___predicate), (void*)L_1);
		Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* L_2 = ___2_selector;
		__this->___selector = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___selector), (void*)L_2);
		return;
	}
}
// Method Definition Index: 100251
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA* WhereSelectListIterator_2_Clone_m1509E7222F78F6B89F836EA7A8742EBDFFC020F9_gshared (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F* __this, const RuntimeMethod* method) 
{
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_0 = __this->___source;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_1 = __this->___predicate;
		Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* L_2 = __this->___selector;
		WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F* L_3 = (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 3));
		WhereSelectListIterator_2__ctor_mDB747029572F65F32894254A0E779C445B0B1E15(L_3, L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		return (Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)L_3;
	}
}
// Method Definition Index: 100252
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WhereSelectListIterator_2_MoveNext_mF219C7B364CA9F7692179ED01E1588FD7862A585_gshared (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	JsonValue_t01DB320267C848E729A400EF2345979978F851D2 V_1;
	memset((&V_1), 0, sizeof(V_1));
	{
		int32_t L_0 = ((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___state;
		V_0 = L_0;
		int32_t L_1 = V_0;
		if ((((int32_t)L_1) == ((int32_t)1)))
		{
			goto IL_0011;
		}
	}
	{
		int32_t L_2 = V_0;
		if ((((int32_t)L_2) == ((int32_t)2)))
		{
			goto IL_0061;
		}
	}
	{
		goto IL_0074;
	}

IL_0011:
	{
		List_1_t4A80BCCFB0BC8742C8BB601365DB07226750573A* L_3 = __this->___source;
		NullCheck(L_3);
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB L_4;
		L_4 = List_1_GetEnumerator_mE2A3E04FB3B522B90EBAC4EFFF9614F47FE19D13(L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		__this->___enumerator = L_4;
		Il2CppCodeGenWriteBarrier((void**)&(((&__this->___enumerator))->____list), (void*)NULL);
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&((&((&(((&__this->___enumerator))->____current))->___stringValue))->___text))->___m_String), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___arrayValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___objectValue), (void*)NULL);
		#endif
		#if IL2CPP_ENABLE_STRICT_WRITE_BARRIERS
		Il2CppCodeGenWriteBarrier((void**)&((&(((&__this->___enumerator))->____current))->___anyValue), (void*)NULL);
		#endif
		((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___state = 2;
		goto IL_0061;
	}

IL_002b:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_5 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_6;
		L_6 = Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_inline(L_5, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_1 = L_6;
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_7 = __this->___predicate;
		if (!L_7)
		{
			goto IL_004d;
		}
	}
	{
		Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* L_8 = __this->___predicate;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_9 = V_1;
		NullCheck(L_8);
		bool L_10;
		L_10 = Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_inline(L_8, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		if (!L_10)
		{
			goto IL_0061;
		}
	}

IL_004d:
	{
		Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* L_11 = __this->___selector;
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_12 = V_1;
		NullCheck(L_11);
		RuntimeObject* L_13;
		L_13 = Func_2_Invoke_mCF21D6903C1C9AB285FB0E244CDE0E9ACAC429ED_inline(L_11, L_12, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___current = L_13;
		Il2CppCodeGenWriteBarrier((void**)(&((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this)->___current), (void*)L_13);
		return (bool)1;
	}

IL_0061:
	{
		Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* L_14 = (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB*)(&__this->___enumerator);
		bool L_15;
		L_15 = Enumerator_MoveNext_m698365CC16BD65E80A0737FD26ED23F19C8AE5DF(L_14, il2cpp_rgctx_method(method->klass->rgctx_data, 16));
		if (L_15)
		{
			goto IL_002b;
		}
	}
	{
		NullCheck((Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this);
		VirtualActionInvoker0::Invoke(12, (Iterator_1_t99A1802EE86A3D5BF71B2DDB37F159C4AFA448EA*)__this);
	}

IL_0074:
	{
		return (bool)0;
	}
}
// Method Definition Index: 100254
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* WhereSelectListIterator_2_Where_mF790B9A9E8480C2654B0FA2B916F50565B9831F1_gshared (WhereSelectListIterator_2_t96704D85C19313BF5304499CA0648C85A9DA5C4F* __this, Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00* ___0_predicate, const RuntimeMethod* method) 
{
	{
		Func_2_tE1F0D41563EE092E5E5540B061449FDE88F1DC00* L_0 = ___0_predicate;
		WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4* L_1 = (WhereEnumerableIterator_1_t1E787D13759F5A31C94B3FAED181402B25C278F4*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 19));
		WhereEnumerableIterator_1__ctor_m8C0DA4CDA5431C03561F67C4393BB18CDD891F01(L_1, (RuntimeObject*)__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 20));
		return (RuntimeObject*)L_1;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 106001
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* WitEntityDataBase_1_FromEntityWitResponseNode_m0AACEFF9E3C6FEA0A185EA5A1B1B4EF5FB491FBB_gshared (WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* __this, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___0_node, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:39>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_0 = ___0_node;
		__this->___responseNode = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___responseNode), (void*)L_0);
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:40>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_1 = ___0_node;
		il2cpp_codegen_runtime_class_init_inline(JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* L_2;
		L_2 = JsonConvert_DeserializeIntoObject_TisWitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507_m0FD2F6631293E41A0DAD13F38DD6D011B88E37B0(__this, L_1, (JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*)NULL, (bool)0, il2cpp_rgctx_method(method->klass->rgctx_data, 1));
		return L_2;
	}
}
// Method Definition Index: 106002
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* WitEntityDataBase_1_ToString_m589391B12E05BAAED6C3E9BD7971B2A9ABB61855_gshared (WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:45>
		int32_t* L_0 = (int32_t*)(&__this->___value);
		String_t* L_1;
		L_1 = Int32_ToString_m030E01C24E294D6762FB0B6F37CB541581F55CA5(L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 3));
		return L_1;
	}
}
// Method Definition Index: 106003
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WitEntityDataBase_1__ctor_m47659B0D37AAA587A516AB8F63573058A00A225D_gshared (WitEntityDataBase_1_t46F92AF5EC7866816B6CD6D417773D4FFE270507* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 106001
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* WitEntityDataBase_1_FromEntityWitResponseNode_m86F61120798C478643AF4031DE9CC7CE890E7DF1_gshared (WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* __this, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___0_node, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:39>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_0 = ___0_node;
		__this->___responseNode = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___responseNode), (void*)L_0);
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:40>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_1 = ___0_node;
		il2cpp_codegen_runtime_class_init_inline(JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* L_2;
		L_2 = JsonConvert_DeserializeIntoObject_TisWitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A_m98EB4FF531C614B59CCD6BC277153895BDF2EA48(__this, L_1, (JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*)NULL, (bool)0, il2cpp_rgctx_method(method->klass->rgctx_data, 1));
		return L_2;
	}
}
// Method Definition Index: 106002
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* WitEntityDataBase_1_ToString_mFBAE16E1599FD53C4310CC682EA6A6D77708CD48_gshared (WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:45>
		RuntimeObject** L_0 = (RuntimeObject**)(&__this->___value);
		NullCheck((*L_0));
		String_t* L_1;
		L_1 = VirtualFuncInvoker0< String_t* >::Invoke(3, (*L_0));
		return L_1;
	}
}
// Method Definition Index: 106003
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WitEntityDataBase_1__ctor_m5C970DAB1C9B01D04F0BD6D9B2FCE291BF11ACF9_gshared (WitEntityDataBase_1_t3ECCF60AD343C4AF152B57E138A9D0F8A8D94C0A* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 106001
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* WitEntityDataBase_1_FromEntityWitResponseNode_m697E7D61EF38859B86A67FAC7B6062E20F5C91B0_gshared (WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* __this, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___0_node, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:39>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_0 = ___0_node;
		__this->___responseNode = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___responseNode), (void*)L_0);
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:40>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_1 = ___0_node;
		il2cpp_codegen_runtime_class_init_inline(JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* L_2;
		L_2 = JsonConvert_DeserializeIntoObject_TisWitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960_mB99E5FCA35B6DB26C561A8A56D5B255D3097D113(__this, L_1, (JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*)NULL, (bool)0, il2cpp_rgctx_method(method->klass->rgctx_data, 1));
		return L_2;
	}
}
// Method Definition Index: 106002
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* WitEntityDataBase_1_ToString_m559746678D2E800FBF20AA902184BB03ABD5963E_gshared (WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:45>
		float* L_0 = (float*)(&__this->___value);
		String_t* L_1;
		L_1 = Single_ToString_mE282EDA9CA4F7DF88432D807732837A629D04972(L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 3));
		return L_1;
	}
}
// Method Definition Index: 106003
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WitEntityDataBase_1__ctor_m02EB7DBBEE819293BC6F42383C89E3CABC514E3F_gshared (WitEntityDataBase_1_t90E9F0FB214EED844AD8DD1A97EA3C1FD1EED960* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 106001
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6* WitEntityDataBase_1_FromEntityWitResponseNode_m2A96C59D1D12FDCB580D1C1F2F77E5978FA7B7CD_gshared (WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6* __this, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* ___0_node, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:39>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_0 = ___0_node;
		il2cpp_codegen_write_instance_field_data<WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C*>(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 0),0), L_0);
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:40>
		WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C* L_1 = ___0_node;
		il2cpp_codegen_runtime_class_init_inline(JsonConvert_t69A348E775F1FA63C310F5FE324F3EE231AD8072_il2cpp_TypeInfo_var);
		WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6* L_2;
		L_2 = InvokerFuncInvoker4< WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6*, WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6*, WitResponseNode_t1BFF810F15B21416F5F6D0A1CAAF03CEF942038C*, JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*, bool >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 1)), il2cpp_rgctx_method(method->klass->rgctx_data, 1), NULL, __this, L_1, (JsonConverterU5BU5D_tF117D3A16A2A7AAC2A66A7EDCD32E3F48B704DDA*)NULL, (bool)0);
		return L_2;
	}
}
// Method Definition Index: 106002
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* WitEntityDataBase_1_ToString_m2EDC565705637A5D658625C85550EC6591E2878C_gshared (WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6* __this, const RuntimeMethod* method) 
{
	void* L_0 = alloca(Il2CppFakeBoxBuffer::SizeNeededFor(il2cpp_rgctx_data(method->klass->rgctx_data, 2)));
	{
		//<source_info:./Library/PackageCache/com.meta.xr.sdk.voice@f4180b2de811/Lib/Wit.ai/Scripts/Runtime/Data/Entities/WitEntityData.cs:45>
		String_t* L_1;
		L_1 = ConstrainedFuncInvoker0< String_t* >::Invoke(il2cpp_rgctx_data(method->klass->rgctx_data, 2), il2cpp_rgctx_method(method->klass->rgctx_data, 3), L_0, (void*)(((Il2CppFullySharedGenericAny*)il2cpp_codegen_get_instance_field_data_pointer(__this, il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 0),8)))));
		return L_1;
	}
}
// Method Definition Index: 106003
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WitEntityDataBase_1__ctor_m262E6B88066F73CF24A328EBCEDEB83B1357B84B_gshared (WitEntityDataBase_1_tC582BAA52A92F908A683443BC6CEBCBAC5ECD6F6* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 75534
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_m8B26C5F2B3AB3C05CB4ACDE5A17C6559BDEF503C_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:108>
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_0 = ___0_src;
		int32_t L_1 = ___1_srcLen;
		WorkSlice_1__ctor_mF7CEAE61925236C3A0BE9E92E9B97B65EF5DFFEF(__this, L_0, 0, L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:108>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1__ctor_m8B26C5F2B3AB3C05CB4ACDE5A17C6559BDEF503C_AdjustorThunk (RuntimeObject* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method)
{
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*>(__this + _offset);
	WorkSlice_1__ctor_m8B26C5F2B3AB3C05CB4ACDE5A17C6559BDEF503C(_thisAdjusted, ___0_src, ___1_srcLen, method);
}
// Method Definition Index: 75535
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_mF7CEAE61925236C3A0BE9E92E9B97B65EF5DFFEF_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* G_B2_0 = NULL;
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* G_B1_0 = NULL;
	int32_t G_B3_0 = 0;
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* G_B3_1 = NULL;
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:112>
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_0 = ___0_src;
		__this->___m_Data = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___m_Data), (void*)L_0);
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:113>
		int32_t L_1 = ___1_srcStart;
		__this->___m_Start = L_1;
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:114>
		int32_t L_2 = ___2_srcLen;
		if ((((int32_t)L_2) < ((int32_t)0)))
		{
			G_B2_0 = __this;
			goto IL_001e;
		}
		G_B1_0 = __this;
	}
	{
		int32_t L_3 = ___2_srcLen;
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_4 = ___0_src;
		NullCheck(L_4);
		il2cpp_codegen_runtime_class_init_inline(Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		int32_t L_5;
		L_5 = Math_Min_m53C488772A34D53917BCA2A491E79A0A5356ED52(L_3, ((int32_t)(((RuntimeArray*)L_4)->max_length)), NULL);
		G_B3_0 = L_5;
		G_B3_1 = G_B1_0;
		goto IL_0021;
	}

IL_001e:
	{
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_6 = ___0_src;
		NullCheck(L_6);
		G_B3_0 = ((int32_t)(((RuntimeArray*)L_6)->max_length));
		G_B3_1 = G_B2_0;
	}

IL_0021:
	{
		G_B3_1->___m_Length = G_B3_0;
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:116>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1__ctor_mF7CEAE61925236C3A0BE9E92E9B97B65EF5DFFEF_AdjustorThunk (RuntimeObject* __this, Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method)
{
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*>(__this + _offset);
	WorkSlice_1__ctor_mF7CEAE61925236C3A0BE9E92E9B97B65EF5DFFEF(_thisAdjusted, ___0_src, ___1_srcStart, ___2_srcLen, method);
}
// Method Definition Index: 75536
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 WorkSlice_1_get_Item_m31D876CE45C89B17D24E2D2EB6534C4150D72EE5_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, int32_t ___0_index, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:120>
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_0 = __this->___m_Data;
		int32_t L_1 = __this->___m_Start;
		int32_t L_2 = ___0_index;
		NullCheck(L_0);
		int32_t L_3 = ((int32_t)il2cpp_codegen_add(L_1, L_2));
		Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 L_4 = (L_0)->GetAt(static_cast<il2cpp_array_size_t>(L_3));
		return L_4;
	}
}
IL2CPP_EXTERN_C  Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 WorkSlice_1_get_Item_m31D876CE45C89B17D24E2D2EB6534C4150D72EE5_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*>(__this + _offset);
	Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 _returnValue;
	_returnValue = WorkSlice_1_get_Item_m31D876CE45C89B17D24E2D2EB6534C4150D72EE5(_thisAdjusted, ___0_index, method);
	return _returnValue;
}
// Method Definition Index: 75537
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_set_Item_m242025FA56F79603361582298B70AB2284A12067_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, int32_t ___0_index, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___1_value, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:121>
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_0 = __this->___m_Data;
		int32_t L_1 = __this->___m_Start;
		int32_t L_2 = ___0_index;
		Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 L_3 = ___1_value;
		NullCheck(L_0);
		(L_0)->SetAt(static_cast<il2cpp_array_size_t>(((int32_t)il2cpp_codegen_add(L_1, L_2))), (Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3)L_3);
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1_set_Item_m242025FA56F79603361582298B70AB2284A12067_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3 ___1_value, const RuntimeMethod* method)
{
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*>(__this + _offset);
	WorkSlice_1_set_Item_m242025FA56F79603361582298B70AB2284A12067(_thisAdjusted, ___0_index, ___1_value, method);
}
// Method Definition Index: 75538
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_mCDE5BAF472BC1BEBC9D091F532AC1A07D65DB0BC_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:124>
		int32_t L_0 = __this->___m_Length;
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t WorkSlice_1_get_length_mCDE5BAF472BC1BEBC9D091F532AC1A07D65DB0BC_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*>(__this + _offset);
	int32_t _returnValue;
	_returnValue = WorkSlice_1_get_length_mCDE5BAF472BC1BEBC9D091F532AC1A07D65DB0BC_inline(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 75539
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_capacity_m514E3E1482974088A807521F9E2C481EB83C25F8_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:125>
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_0 = __this->___m_Data;
		NullCheck(L_0);
		return ((int32_t)(((RuntimeArray*)L_0)->max_length));
	}
}
IL2CPP_EXTERN_C  int32_t WorkSlice_1_get_capacity_m514E3E1482974088A807521F9E2C481EB83C25F8_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*>(__this + _offset);
	int32_t _returnValue;
	_returnValue = WorkSlice_1_get_capacity_m514E3E1482974088A807521F9E2C481EB83C25F8(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 75540
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_Sort_m9DF994BABD5BF00DE8FEB14D82F7A0A9F6CAEE3D_gshared (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F* ___0_compare, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Sorting_tBB4ACAADCAA21EA710DD3998A0614ABDEF8FD8A6_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:129>
		int32_t L_0 = __this->___m_Length;
		if ((((int32_t)L_0) <= ((int32_t)1)))
		{
			goto IL_002a;
		}
	}
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:130>
		Vector4U5BU5D_tC0F3A7115F85007510F6D173968200CD31BCF7AD* L_1 = __this->___m_Data;
		int32_t L_2 = __this->___m_Start;
		int32_t L_3 = __this->___m_Start;
		int32_t L_4 = __this->___m_Length;
		Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F* L_5 = ___0_compare;
		il2cpp_codegen_runtime_class_init_inline(Sorting_tBB4ACAADCAA21EA710DD3998A0614ABDEF8FD8A6_il2cpp_TypeInfo_var);
		Sorting_QuickSort_TisVector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3_mB8556CFA6B9237741AAA1ADF2EE68AA3696F9477(L_1, L_2, ((int32_t)il2cpp_codegen_subtract(((int32_t)il2cpp_codegen_add(L_3, L_4)), 1)), L_5, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 6));
	}

IL_002a:
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:131>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1_Sort_m9DF994BABD5BF00DE8FEB14D82F7A0A9F6CAEE3D_AdjustorThunk (RuntimeObject* __this, Func_3_tD46831209E6E19204CD8F9EAFC74DBFCA3C36C5F* ___0_compare, const RuntimeMethod* method)
{
	WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44*>(__this + _offset);
	WorkSlice_1_Sort_m9DF994BABD5BF00DE8FEB14D82F7A0A9F6CAEE3D(_thisAdjusted, ___0_compare, method);
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 75534
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_mFEB81358558CEF0264CCE077535DB880EA2492BA_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:108>
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_0 = ___0_src;
		int32_t L_1 = ___1_srcLen;
		((  void (*) (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*, int32_t, int32_t, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 1)))(__this, L_0, 0, L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:108>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1__ctor_mFEB81358558CEF0264CCE077535DB880EA2492BA_AdjustorThunk (RuntimeObject* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method)
{
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*>(__this + _offset);
	WorkSlice_1__ctor_mFEB81358558CEF0264CCE077535DB880EA2492BA(_thisAdjusted, ___0_src, ___1_srcLen, method);
}
// Method Definition Index: 75535
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_m7AC3CC7AABC83B76D23D2B94329DD4D0200156FA_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* G_B2_0 = NULL;
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* G_B1_0 = NULL;
	int32_t G_B3_0 = 0;
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* G_B3_1 = NULL;
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:112>
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_0 = ___0_src;
		__this->___m_Data = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___m_Data), (void*)L_0);
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:113>
		int32_t L_1 = ___1_srcStart;
		__this->___m_Start = L_1;
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:114>
		int32_t L_2 = ___2_srcLen;
		if ((((int32_t)L_2) < ((int32_t)0)))
		{
			G_B2_0 = __this;
			goto IL_001e;
		}
		G_B1_0 = __this;
	}
	{
		int32_t L_3 = ___2_srcLen;
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_4 = ___0_src;
		NullCheck(L_4);
		il2cpp_codegen_runtime_class_init_inline(Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		int32_t L_5;
		L_5 = Math_Min_m53C488772A34D53917BCA2A491E79A0A5356ED52(L_3, ((int32_t)(((RuntimeArray*)L_4)->max_length)), NULL);
		G_B3_0 = L_5;
		G_B3_1 = G_B1_0;
		goto IL_0021;
	}

IL_001e:
	{
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_6 = ___0_src;
		NullCheck(L_6);
		G_B3_0 = ((int32_t)(((RuntimeArray*)L_6)->max_length));
		G_B3_1 = G_B2_0;
	}

IL_0021:
	{
		G_B3_1->___m_Length = G_B3_0;
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:116>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1__ctor_m7AC3CC7AABC83B76D23D2B94329DD4D0200156FA_AdjustorThunk (RuntimeObject* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method)
{
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*>(__this + _offset);
	WorkSlice_1__ctor_m7AC3CC7AABC83B76D23D2B94329DD4D0200156FA(_thisAdjusted, ___0_src, ___1_srcStart, ___2_srcLen, method);
}
// Method Definition Index: 75536
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_get_Item_mFDEC427E08156ECD6879AD45AEE3618B43E3F726_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, int32_t ___0_index, Il2CppFullySharedGenericAny* il2cppRetVal, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 4));
	const Il2CppFullySharedGenericAny L_4 = alloca(SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A);
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:120>
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_0 = __this->___m_Data;
		int32_t L_1 = __this->___m_Start;
		int32_t L_2 = ___0_index;
		NullCheck(L_0);
		int32_t L_3 = ((int32_t)il2cpp_codegen_add(L_1, L_2));
		il2cpp_codegen_memcpy(L_4, (L_0)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_3)), SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A);
		il2cpp_codegen_memcpy(il2cppRetVal, L_4, SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A);
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1_get_Item_mFDEC427E08156ECD6879AD45AEE3618B43E3F726_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, Il2CppFullySharedGenericAny* il2cppRetVal, const RuntimeMethod* method)
{
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*>(__this + _offset);
	WorkSlice_1_get_Item_mFDEC427E08156ECD6879AD45AEE3618B43E3F726(_thisAdjusted, ___0_index, il2cppRetVal, method);
	return;
}
// Method Definition Index: 75537
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_set_Item_m757F8BE76FAE27C149DE7C474A2B1845E08A5A0F_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, int32_t ___0_index, Il2CppFullySharedGenericAny ___1_value, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 4));
	const Il2CppFullySharedGenericAny L_3 = alloca(SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A);
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:121>
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_0 = __this->___m_Data;
		int32_t L_1 = __this->___m_Start;
		int32_t L_2 = ___0_index;
		il2cpp_codegen_memcpy(L_3, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(InitializedTypeInfo(method->klass)->rgctx_data, 4)) ? ___1_value : &___1_value), SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A);
		NullCheck(L_0);
		il2cpp_codegen_memcpy((L_0)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)il2cpp_codegen_add(L_1, L_2)))), L_3, SizeOf_T_tA71336896536EBCB168400E4D351FEE422324E7A);
		Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(InitializedTypeInfo(method->klass)->rgctx_data, 4), (void**)(L_0)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)il2cpp_codegen_add(L_1, L_2)))), (void*)L_3);
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1_set_Item_m757F8BE76FAE27C149DE7C474A2B1845E08A5A0F_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, Il2CppFullySharedGenericAny ___1_value, const RuntimeMethod* method)
{
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*>(__this + _offset);
	WorkSlice_1_set_Item_m757F8BE76FAE27C149DE7C474A2B1845E08A5A0F(_thisAdjusted, ___0_index, ___1_value, method);
}
// Method Definition Index: 75538
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_m7212817506EACBA1AB0D914DE401C6FA05F0DD9D_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:124>
		int32_t L_0 = __this->___m_Length;
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t WorkSlice_1_get_length_m7212817506EACBA1AB0D914DE401C6FA05F0DD9D_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*>(__this + _offset);
	int32_t _returnValue;
	_returnValue = WorkSlice_1_get_length_m7212817506EACBA1AB0D914DE401C6FA05F0DD9D_inline(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 75539
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_capacity_mCF0D603B7EC6E6037D0E1A14D8D0F49AD063E59E_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:125>
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_0 = __this->___m_Data;
		NullCheck(L_0);
		return ((int32_t)(((RuntimeArray*)L_0)->max_length));
	}
}
IL2CPP_EXTERN_C  int32_t WorkSlice_1_get_capacity_mCF0D603B7EC6E6037D0E1A14D8D0F49AD063E59E_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*>(__this + _offset);
	int32_t _returnValue;
	_returnValue = WorkSlice_1_get_capacity_mCF0D603B7EC6E6037D0E1A14D8D0F49AD063E59E(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 75540
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_Sort_mE2ED392BDF8F4F4141D7BF4C984EFE943F607A94_gshared (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9* ___0_compare, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Sorting_tBB4ACAADCAA21EA710DD3998A0614ABDEF8FD8A6_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:129>
		int32_t L_0 = __this->___m_Length;
		if ((((int32_t)L_0) <= ((int32_t)1)))
		{
			goto IL_002a;
		}
	}
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:130>
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_1 = __this->___m_Data;
		int32_t L_2 = __this->___m_Start;
		int32_t L_3 = __this->___m_Start;
		int32_t L_4 = __this->___m_Length;
		Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9* L_5 = ___0_compare;
		il2cpp_codegen_runtime_class_init_inline(Sorting_tBB4ACAADCAA21EA710DD3998A0614ABDEF8FD8A6_il2cpp_TypeInfo_var);
		((  void (*) (__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*, int32_t, int32_t, Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 6)))(L_1, L_2, ((int32_t)il2cpp_codegen_subtract(((int32_t)il2cpp_codegen_add(L_3, L_4)), 1)), L_5, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 6));
	}

IL_002a:
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:131>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1_Sort_mE2ED392BDF8F4F4141D7BF4C984EFE943F607A94_AdjustorThunk (RuntimeObject* __this, Func_3_tECED1961B53AB164A131061296ABA1276B4FBBB9* ___0_compare, const RuntimeMethod* method)
{
	WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809*>(__this + _offset);
	WorkSlice_1_Sort_mE2ED392BDF8F4F4141D7BF4C984EFE943F607A94(_thisAdjusted, ___0_compare, method);
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 75534
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_m76CCC65E3DFB8B84A2154B65A79B56688F9B26A4_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:108>
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_0 = ___0_src;
		int32_t L_1 = ___1_srcLen;
		WorkSlice_1__ctor_mC3BCAA8301A4E37DDB26811AAFFE1A3654FA47D0(__this, L_0, 0, L_1, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 1));
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:108>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1__ctor_m76CCC65E3DFB8B84A2154B65A79B56688F9B26A4_AdjustorThunk (RuntimeObject* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcLen, const RuntimeMethod* method)
{
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*>(__this + _offset);
	WorkSlice_1__ctor_m76CCC65E3DFB8B84A2154B65A79B56688F9B26A4(_thisAdjusted, ___0_src, ___1_srcLen, method);
}
// Method Definition Index: 75535
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1__ctor_mC3BCAA8301A4E37DDB26811AAFFE1A3654FA47D0_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* G_B2_0 = NULL;
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* G_B1_0 = NULL;
	int32_t G_B3_0 = 0;
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* G_B3_1 = NULL;
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:112>
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_0 = ___0_src;
		__this->___m_Data = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___m_Data), (void*)L_0);
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:113>
		int32_t L_1 = ___1_srcStart;
		__this->___m_Start = L_1;
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:114>
		int32_t L_2 = ___2_srcLen;
		if ((((int32_t)L_2) < ((int32_t)0)))
		{
			G_B2_0 = __this;
			goto IL_001e;
		}
		G_B1_0 = __this;
	}
	{
		int32_t L_3 = ___2_srcLen;
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_4 = ___0_src;
		NullCheck(L_4);
		il2cpp_codegen_runtime_class_init_inline(Math_tEB65DE7CA8B083C412C969C92981C030865486CE_il2cpp_TypeInfo_var);
		int32_t L_5;
		L_5 = Math_Min_m53C488772A34D53917BCA2A491E79A0A5356ED52(L_3, ((int32_t)(((RuntimeArray*)L_4)->max_length)), NULL);
		G_B3_0 = L_5;
		G_B3_1 = G_B1_0;
		goto IL_0021;
	}

IL_001e:
	{
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_6 = ___0_src;
		NullCheck(L_6);
		G_B3_0 = ((int32_t)(((RuntimeArray*)L_6)->max_length));
		G_B3_1 = G_B2_0;
	}

IL_0021:
	{
		G_B3_1->___m_Length = G_B3_0;
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:116>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1__ctor_mC3BCAA8301A4E37DDB26811AAFFE1A3654FA47D0_AdjustorThunk (RuntimeObject* __this, LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* ___0_src, int32_t ___1_srcStart, int32_t ___2_srcLen, const RuntimeMethod* method)
{
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*>(__this + _offset);
	WorkSlice_1__ctor_mC3BCAA8301A4E37DDB26811AAFFE1A3654FA47D0(_thisAdjusted, ___0_src, ___1_srcStart, ___2_srcLen, method);
}
// Method Definition Index: 75536
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 WorkSlice_1_get_Item_mFD8EE50B88077F3EF9BCEF97BD6D0352D2E8445D_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, int32_t ___0_index, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:120>
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_0 = __this->___m_Data;
		int32_t L_1 = __this->___m_Start;
		int32_t L_2 = ___0_index;
		NullCheck(L_0);
		int32_t L_3 = ((int32_t)il2cpp_codegen_add(L_1, L_2));
		LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 L_4 = (L_0)->GetAt(static_cast<il2cpp_array_size_t>(L_3));
		return L_4;
	}
}
IL2CPP_EXTERN_C  LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 WorkSlice_1_get_Item_mFD8EE50B88077F3EF9BCEF97BD6D0352D2E8445D_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, const RuntimeMethod* method)
{
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*>(__this + _offset);
	LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 _returnValue;
	_returnValue = WorkSlice_1_get_Item_mFD8EE50B88077F3EF9BCEF97BD6D0352D2E8445D(_thisAdjusted, ___0_index, method);
	return _returnValue;
}
// Method Definition Index: 75537
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_set_Item_m070889CA2F62E82384BAB3CEF6D6E9AABF153663_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, int32_t ___0_index, LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 ___1_value, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:121>
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_0 = __this->___m_Data;
		int32_t L_1 = __this->___m_Start;
		int32_t L_2 = ___0_index;
		LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 L_3 = ___1_value;
		NullCheck(L_0);
		(L_0)->SetAt(static_cast<il2cpp_array_size_t>(((int32_t)il2cpp_codegen_add(L_1, L_2))), (LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2)L_3);
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1_set_Item_m070889CA2F62E82384BAB3CEF6D6E9AABF153663_AdjustorThunk (RuntimeObject* __this, int32_t ___0_index, LightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2 ___1_value, const RuntimeMethod* method)
{
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*>(__this + _offset);
	WorkSlice_1_set_Item_m070889CA2F62E82384BAB3CEF6D6E9AABF153663(_thisAdjusted, ___0_index, ___1_value, method);
}
// Method Definition Index: 75538
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_m0E5769CB5B052657E7327DCAD0F2CA104327D7D8_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:124>
		int32_t L_0 = __this->___m_Length;
		return L_0;
	}
}
IL2CPP_EXTERN_C  int32_t WorkSlice_1_get_length_m0E5769CB5B052657E7327DCAD0F2CA104327D7D8_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*>(__this + _offset);
	int32_t _returnValue;
	_returnValue = WorkSlice_1_get_length_m0E5769CB5B052657E7327DCAD0F2CA104327D7D8_inline(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 75539
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_capacity_m98A237D126494A11FF3C61211B36A790E4E8A3B0_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:125>
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_0 = __this->___m_Data;
		NullCheck(L_0);
		return ((int32_t)(((RuntimeArray*)L_0)->max_length));
	}
}
IL2CPP_EXTERN_C  int32_t WorkSlice_1_get_capacity_m98A237D126494A11FF3C61211B36A790E4E8A3B0_AdjustorThunk (RuntimeObject* __this, const RuntimeMethod* method)
{
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*>(__this + _offset);
	int32_t _returnValue;
	_returnValue = WorkSlice_1_get_capacity_m98A237D126494A11FF3C61211B36A790E4E8A3B0(_thisAdjusted, method);
	return _returnValue;
}
// Method Definition Index: 75540
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkSlice_1_Sort_m7EF532E936D55845DAAC606C0A214FE48EBF8584_gshared (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___0_compare, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Sorting_tBB4ACAADCAA21EA710DD3998A0614ABDEF8FD8A6_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:129>
		int32_t L_0 = __this->___m_Length;
		if ((((int32_t)L_0) <= ((int32_t)1)))
		{
			goto IL_002a;
		}
	}
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:130>
		LightCookieMappingU5BU5D_tE1F10A7D54920D3636F1DB7774B3D5F5B560E263* L_1 = __this->___m_Data;
		int32_t L_2 = __this->___m_Start;
		int32_t L_3 = __this->___m_Start;
		int32_t L_4 = __this->___m_Length;
		Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* L_5 = ___0_compare;
		il2cpp_codegen_runtime_class_init_inline(Sorting_tBB4ACAADCAA21EA710DD3998A0614ABDEF8FD8A6_il2cpp_TypeInfo_var);
		Sorting_QuickSort_TisLightCookieMapping_t76B317D9FDE96056FA698B46B45D7F0937BD02D2_mD7019ED48D20810C1169430283118F96945E9450(L_1, L_2, ((int32_t)il2cpp_codegen_subtract(((int32_t)il2cpp_codegen_add(L_3, L_4)), 1)), L_5, il2cpp_rgctx_method(InitializedTypeInfo(method->klass)->rgctx_data, 6));
	}

IL_002a:
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:131>
		return;
	}
}
IL2CPP_EXTERN_C  void WorkSlice_1_Sort_m7EF532E936D55845DAAC606C0A214FE48EBF8584_AdjustorThunk (RuntimeObject* __this, Func_3_t3E644C82345CE04737DA5CDE018481FE0A88F821* ___0_compare, const RuntimeMethod* method)
{
	WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* _thisAdjusted;
	int32_t _offset = 1;
	_thisAdjusted = reinterpret_cast<WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87*>(__this + _offset);
	WorkSlice_1_Sort_m7EF532E936D55845DAAC606C0A214FE48EBF8584(_thisAdjusted, ___0_compare, method);
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 65451
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkStealingQueue__ctor_m5008A780D3A45C6DD9B8248FDD47AA3766CBD9E3_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* ___0_nextQueue, const RuntimeMethod* method) 
{
	{
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_0 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(il2cpp_rgctx_data(method->klass->rgctx_data, 0), (uint32_t)((int32_t)32));
		il2cpp_codegen_memory_barrier();
		__this->____array = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____array), (void*)L_0);
		il2cpp_codegen_memory_barrier();
		__this->____mask = ((int32_t)31);
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		int32_t L_1;
		L_1 = Environment_get_CurrentManagedThreadId_m66483AADCCC13272EBDCD94D31D2E52603C24BDF(NULL);
		__this->____ownerThreadId = L_1;
		WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* L_2 = ___0_nextQueue;
		__this->____nextQueue = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____nextQueue), (void*)L_2);
		return;
	}
}
// Method Definition Index: 65452
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkStealingQueue_LocalPush_m8B1D8601B4870C599A8FF4F36500D74B9C159C0F_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, RuntimeObject* ___0_item, int64_t* ___1_emptyToNonEmptyListTransitionCount, const RuntimeMethod* method) 
{
	bool V_0 = false;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* V_3 = NULL;
	bool V_4 = false;
	int32_t V_5 = 0;
	ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* V_6 = NULL;
	int32_t V_7 = 0;
	{
		V_0 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_0203:
			{
				{
					il2cpp_codegen_memory_barrier();
					__this->____currentOp = 0;
					bool L_0 = V_0;
					if (!L_0)
					{
						goto IL_0215;
					}
				}
				{
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)__this, NULL);
				}

IL_0215:
				{
					return;
				}
			}
		});
		try
		{
			{
				int32_t* L_1 = (int32_t*)(&__this->____currentOp);
				il2cpp_codegen_memory_barrier();
				int32_t L_2;
				L_2 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_1, 1, NULL);
				int32_t L_3 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				V_1 = L_3;
				int32_t L_4 = V_1;
				if ((!(((uint32_t)L_4) == ((uint32_t)((int32_t)2147483647LL)))))
				{
					goto IL_007d_1;
				}
			}
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 0;
				V_3 = __this;
				V_4 = (bool)0;
			}
			{
				auto __finallyBlock = il2cpp::utils::Finally([&]
				{

FINALLY_0072_1:
					{
						{
							bool L_5 = V_4;
							if (!L_5)
							{
								goto IL_007c_1;
							}
						}
						{
							WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* L_6 = V_3;
							Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_6, NULL);
						}

IL_007c_1:
						{
							return;
						}
					}
				});
				try
				{
					WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* L_7 = V_3;
					Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_7, (&V_4), NULL);
					int32_t L_8 = __this->____headIndex;
					il2cpp_codegen_memory_barrier();
					int32_t L_9 = __this->____mask;
					il2cpp_codegen_memory_barrier();
					il2cpp_codegen_memory_barrier();
					__this->____headIndex = ((int32_t)(L_8&L_9));
					int32_t L_10 = V_1;
					int32_t L_11 = __this->____mask;
					il2cpp_codegen_memory_barrier();
					int32_t L_12 = ((int32_t)(L_10&L_11));
					V_1 = L_12;
					il2cpp_codegen_memory_barrier();
					__this->____tailIndex = L_12;
					int32_t* L_13 = (int32_t*)(&__this->____currentOp);
					il2cpp_codegen_memory_barrier();
					int32_t L_14;
					L_14 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_13, 1, NULL);
					goto IL_007d_1;
				}
				catch(Il2CppExceptionWrapper& e)
				{
					__finallyBlock.StoreException(e.ex);
				}
			}

IL_007d_1:
			{
				int32_t L_15 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				V_2 = L_15;
				bool L_16 = __this->____frozen;
				if (L_16)
				{
					goto IL_00cc_1;
				}
			}
			{
				int32_t L_17 = V_2;
				int32_t L_18 = V_1;
				int32_t L_19 = V_1;
				int32_t L_20 = V_2;
				int32_t L_21 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				if (!((int32_t)(((((int32_t)L_17) < ((int32_t)((int32_t)il2cpp_codegen_subtract(L_18, 1))))? 1 : 0)&((((int32_t)L_19) < ((int32_t)((int32_t)il2cpp_codegen_add(L_20, L_21))))? 1 : 0))))
				{
					goto IL_00cc_1;
				}
			}
			{
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_22 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_23 = V_1;
				int32_t L_24 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				RuntimeObject* L_25 = ___0_item;
				NullCheck(L_22);
				(L_22)->SetAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_23&L_24))), (RuntimeObject*)L_25);
				int32_t L_26 = V_1;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = ((int32_t)il2cpp_codegen_add(L_26, 1));
				goto IL_01f3_1;
			}

IL_00cc_1:
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)__this, (&V_0), NULL);
				int32_t L_27 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				V_2 = L_27;
				int32_t L_28 = V_1;
				int32_t L_29 = V_2;
				V_5 = ((int32_t)il2cpp_codegen_subtract(L_28, L_29));
				int32_t L_30 = V_5;
				int32_t L_31 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_30) < ((int32_t)L_31)))
				{
					goto IL_01ab_1;
				}
			}
			{
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_32 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_32);
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_33 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(il2cpp_rgctx_data(method->klass->rgctx_data, 0), (uint32_t)((int32_t)(((int32_t)(((RuntimeArray*)L_32)->max_length))<<1)));
				V_6 = L_33;
				int32_t L_34 = V_2;
				int32_t L_35 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_7 = ((int32_t)(L_34&L_35));
				int32_t L_36 = V_7;
				if (L_36)
				{
					goto IL_013a_1;
				}
			}
			{
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_37 = __this->____array;
				il2cpp_codegen_memory_barrier();
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_38 = V_6;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_39 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_39);
				Array_Copy_mB4904E17BD92E320613A3251C0205E0786B3BF41((RuntimeArray*)L_37, 0, (RuntimeArray*)L_38, 0, ((int32_t)(((RuntimeArray*)L_39)->max_length)), NULL);
				goto IL_0178_1;
			}

IL_013a_1:
			{
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_40 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_41 = V_7;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_42 = V_6;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_43 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_43);
				int32_t L_44 = V_7;
				Array_Copy_mB4904E17BD92E320613A3251C0205E0786B3BF41((RuntimeArray*)L_40, L_41, (RuntimeArray*)L_42, 0, ((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_43)->max_length)), L_44)), NULL);
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_45 = __this->____array;
				il2cpp_codegen_memory_barrier();
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_46 = V_6;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_47 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_47);
				int32_t L_48 = V_7;
				int32_t L_49 = V_7;
				Array_Copy_mB4904E17BD92E320613A3251C0205E0786B3BF41((RuntimeArray*)L_45, 0, (RuntimeArray*)L_46, ((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_47)->max_length)), L_48)), L_49, NULL);
			}

IL_0178_1:
			{
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_50 = V_6;
				il2cpp_codegen_memory_barrier();
				__this->____array = L_50;
				Il2CppCodeGenWriteBarrier((void**)(&__this->____array), (void*)L_50);
				il2cpp_codegen_memory_barrier();
				__this->____headIndex = 0;
				int32_t L_51 = V_5;
				int32_t L_52 = L_51;
				V_1 = L_52;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = L_52;
				int32_t L_53 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				il2cpp_codegen_memory_barrier();
				__this->____mask = ((int32_t)(((int32_t)(L_53<<1))|1));
			}

IL_01ab_1:
			{
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_54 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_55 = V_1;
				int32_t L_56 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				RuntimeObject* L_57 = ___0_item;
				NullCheck(L_54);
				(L_54)->SetAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_55&L_56))), (RuntimeObject*)L_57);
				int32_t L_58 = V_1;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = ((int32_t)il2cpp_codegen_add(L_58, 1));
				int32_t L_59 = V_5;
				if (L_59)
				{
					goto IL_01d9_1;
				}
			}
			{
				int64_t* L_60 = ___1_emptyToNonEmptyListTransitionCount;
				int64_t L_61;
				L_61 = Interlocked_Increment_m12CD643CCD5FCDE95DBC0AB07D09AF696E26F9C0(L_60, NULL);
			}

IL_01d9_1:
			{
				int32_t L_62 = __this->____addTakeCount;
				int32_t L_63 = __this->____stealCount;
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_subtract(L_62, L_63));
				__this->____stealCount = 0;
			}

IL_01f3_1:
			{
				int32_t L_64 = __this->____addTakeCount;
				if (((int64_t)L_64 + (int64_t)1 < (int64_t)kIl2CppInt32Min) || ((int64_t)L_64 + (int64_t)1 > (int64_t)kIl2CppInt32Max))
					IL2CPP_RAISE_MANAGED_EXCEPTION(il2cpp_codegen_get_overflow_exception(), method);
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_add(L_64, 1));
				goto IL_0216;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_0216:
	{
		return;
	}
}
// Method Definition Index: 65453
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkStealingQueue_LocalClear_mDD1FB9622118C08D3361E72CCC198D08BF2AA3E3_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, const RuntimeMethod* method) 
{
	WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* V_0 = NULL;
	bool V_1 = false;
	int32_t V_2 = 0;
	{
		V_0 = __this;
		V_1 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_005c:
			{
				{
					bool L_0 = V_1;
					if (!L_0)
					{
						goto IL_0065;
					}
				}
				{
					WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* L_1 = V_0;
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_1, NULL);
				}

IL_0065:
				{
					return;
				}
			}
		});
		try
		{
			{
				WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* L_2 = V_0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_2, (&V_1), NULL);
				int32_t L_3 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				int32_t L_4 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_3) >= ((int32_t)L_4)))
				{
					goto IL_005a_1;
				}
			}
			{
				int32_t L_5 = 0;
				V_2 = L_5;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = L_5;
				int32_t L_6 = V_2;
				il2cpp_codegen_memory_barrier();
				__this->____headIndex = L_6;
				int32_t L_7 = 0;
				V_2 = L_7;
				__this->____stealCount = L_7;
				int32_t L_8 = V_2;
				__this->____addTakeCount = L_8;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_9 = __this->____array;
				il2cpp_codegen_memory_barrier();
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_10 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_10);
				Array_Clear_m50BAA3751899858B097D3FF2ED31F284703FE5CB((RuntimeArray*)L_9, 0, ((int32_t)(((RuntimeArray*)L_10)->max_length)), NULL);
			}

IL_005a_1:
			{
				goto IL_0066;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_0066:
	{
		return;
	}
}
// Method Definition Index: 65454
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WorkStealingQueue_TryLocalPop_mEE65FB52F67ED82674A4E589A22B445BEC0DC0C1_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, RuntimeObject** ___0_result, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	bool V_1 = false;
	int32_t V_2 = 0;
	RuntimeObject* V_3 = NULL;
	bool V_4 = false;
	int32_t V_5 = 0;
	{
		int32_t L_0 = __this->____tailIndex;
		il2cpp_codegen_memory_barrier();
		V_0 = L_0;
		int32_t L_1 = __this->____headIndex;
		il2cpp_codegen_memory_barrier();
		int32_t L_2 = V_0;
		if ((((int32_t)L_1) < ((int32_t)L_2)))
		{
			goto IL_001d;
		}
	}
	{
		RuntimeObject** L_3 = ___0_result;
		il2cpp_codegen_initobj(L_3, sizeof(RuntimeObject*));
		return (bool)0;
	}

IL_001d:
	{
		V_1 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_0117:
			{
				{
					il2cpp_codegen_memory_barrier();
					__this->____currentOp = 0;
					bool L_4 = V_1;
					if (!L_4)
					{
						goto IL_0129;
					}
				}
				{
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)__this, NULL);
				}

IL_0129:
				{
					return;
				}
			}
		});
		try
		{
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 2;
				int32_t* L_5 = (int32_t*)(&__this->____tailIndex);
				il2cpp_codegen_memory_barrier();
				int32_t L_6 = V_0;
				int32_t L_7 = ((int32_t)il2cpp_codegen_subtract(L_6, 1));
				V_0 = L_7;
				int32_t L_8;
				L_8 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_5, L_7, NULL);
				bool L_9 = __this->____frozen;
				if (L_9)
				{
					goto IL_0098_1;
				}
			}
			{
				int32_t L_10 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				int32_t L_11 = V_0;
				if ((((int32_t)L_10) >= ((int32_t)L_11)))
				{
					goto IL_0098_1;
				}
			}
			{
				int32_t L_12 = V_0;
				int32_t L_13 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_2 = ((int32_t)(L_12&L_13));
				RuntimeObject** L_14 = ___0_result;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_15 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_16 = V_2;
				NullCheck(L_15);
				int32_t L_17 = L_16;
				RuntimeObject* L_18 = (L_15)->GetAt(static_cast<il2cpp_array_size_t>(L_17));
				*(RuntimeObject**)L_14 = L_18;
				Il2CppCodeGenWriteBarrier((void**)(RuntimeObject**)L_14, (void*)L_18);
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_19 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_20 = V_2;
				il2cpp_codegen_initobj((&V_3), sizeof(RuntimeObject*));
				RuntimeObject* L_21 = V_3;
				NullCheck(L_19);
				(L_19)->SetAt(static_cast<il2cpp_array_size_t>(L_20), (RuntimeObject*)L_21);
				int32_t L_22 = __this->____addTakeCount;
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_subtract(L_22, 1));
				V_4 = (bool)1;
				goto IL_012a;
			}

IL_0098_1:
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)__this, (&V_1), NULL);
				int32_t L_23 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				int32_t L_24 = V_0;
				if ((((int32_t)L_23) > ((int32_t)L_24)))
				{
					goto IL_0100_1;
				}
			}
			{
				int32_t L_25 = V_0;
				int32_t L_26 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_5 = ((int32_t)(L_25&L_26));
				RuntimeObject** L_27 = ___0_result;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_28 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_29 = V_5;
				NullCheck(L_28);
				int32_t L_30 = L_29;
				RuntimeObject* L_31 = (L_28)->GetAt(static_cast<il2cpp_array_size_t>(L_30));
				*(RuntimeObject**)L_27 = L_31;
				Il2CppCodeGenWriteBarrier((void**)(RuntimeObject**)L_27, (void*)L_31);
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_32 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_33 = V_5;
				il2cpp_codegen_initobj((&V_3), sizeof(RuntimeObject*));
				RuntimeObject* L_34 = V_3;
				NullCheck(L_32);
				(L_32)->SetAt(static_cast<il2cpp_array_size_t>(L_33), (RuntimeObject*)L_34);
				int32_t L_35 = __this->____addTakeCount;
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_subtract(L_35, 1));
				V_4 = (bool)1;
				goto IL_012a;
			}

IL_0100_1:
			{
				int32_t L_36 = V_0;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = ((int32_t)il2cpp_codegen_add(L_36, 1));
				RuntimeObject** L_37 = ___0_result;
				il2cpp_codegen_initobj(L_37, sizeof(RuntimeObject*));
				V_4 = (bool)0;
				goto IL_012a;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_012a:
	{
		bool L_38 = V_4;
		return L_38;
	}
}
// Method Definition Index: 65455
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WorkStealingQueue_TrySteal_mCB55858C75081104C556E8390D4C2E41E1FAD7D5_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, RuntimeObject** ___0_result, bool ___1_take, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* V_0 = NULL;
	bool V_1 = false;
	int32_t V_2 = 0;
	SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675 V_3;
	memset((&V_3), 0, sizeof(V_3));
	int32_t V_4 = 0;
	RuntimeObject* V_5 = NULL;
	bool V_6 = false;
	{
		V_0 = __this;
		V_1 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_00ee:
			{
				{
					bool L_0 = V_1;
					if (!L_0)
					{
						goto IL_00f7;
					}
				}
				{
					WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* L_1 = V_0;
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_1, NULL);
				}

IL_00f7:
				{
					return;
				}
			}
		});
		try
		{
			{
				WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* L_2 = V_0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_2, (&V_1), NULL);
				int32_t L_3 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				V_2 = L_3;
				bool L_4 = ___1_take;
				if (!L_4)
				{
					goto IL_00bf_1;
				}
			}
			{
				int32_t L_5 = V_2;
				int32_t L_6 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_5) >= ((int32_t)((int32_t)il2cpp_codegen_subtract(L_6, 1)))))
				{
					goto IL_004d_1;
				}
			}
			{
				int32_t L_7 = __this->____currentOp;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_7) == ((int32_t)1)))
				{
					goto IL_004d_1;
				}
			}
			{
				il2cpp_codegen_initobj((&V_3), sizeof(SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675));
			}

IL_003b_1:
			{
				il2cpp_codegen_runtime_class_init_inline(SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675_il2cpp_TypeInfo_var);
				SpinWait_SpinOnce_m5B74E6B15013E90667646C0D943E886D4EC596AF((&V_3), NULL);
				int32_t L_8 = __this->____currentOp;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_8) == ((int32_t)1)))
				{
					goto IL_003b_1;
				}
			}

IL_004d_1:
			{
				int32_t* L_9 = (int32_t*)(&__this->____headIndex);
				il2cpp_codegen_memory_barrier();
				int32_t L_10 = V_2;
				int32_t L_11;
				L_11 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_9, ((int32_t)il2cpp_codegen_add(L_10, 1)), NULL);
				int32_t L_12 = V_2;
				int32_t L_13 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_12) >= ((int32_t)L_13)))
				{
					goto IL_00b4_1;
				}
			}
			{
				int32_t L_14 = V_2;
				int32_t L_15 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_4 = ((int32_t)(L_14&L_15));
				RuntimeObject** L_16 = ___0_result;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_17 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_18 = V_4;
				NullCheck(L_17);
				int32_t L_19 = L_18;
				RuntimeObject* L_20 = (L_17)->GetAt(static_cast<il2cpp_array_size_t>(L_19));
				*(RuntimeObject**)L_16 = L_20;
				Il2CppCodeGenWriteBarrier((void**)(RuntimeObject**)L_16, (void*)L_20);
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_21 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_22 = V_4;
				il2cpp_codegen_initobj((&V_5), sizeof(RuntimeObject*));
				RuntimeObject* L_23 = V_5;
				NullCheck(L_21);
				(L_21)->SetAt(static_cast<il2cpp_array_size_t>(L_22), (RuntimeObject*)L_23);
				int32_t L_24 = __this->____stealCount;
				__this->____stealCount = ((int32_t)il2cpp_codegen_add(L_24, 1));
				V_6 = (bool)1;
				goto IL_0101;
			}

IL_00b4_1:
			{
				int32_t L_25 = V_2;
				il2cpp_codegen_memory_barrier();
				__this->____headIndex = L_25;
				goto IL_00f8;
			}

IL_00bf_1:
			{
				int32_t L_26 = V_2;
				int32_t L_27 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_26) >= ((int32_t)L_27)))
				{
					goto IL_00ec_1;
				}
			}
			{
				RuntimeObject** L_28 = ___0_result;
				ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_29 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_30 = V_2;
				int32_t L_31 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_29);
				int32_t L_32 = ((int32_t)(L_30&L_31));
				RuntimeObject* L_33 = (L_29)->GetAt(static_cast<il2cpp_array_size_t>(L_32));
				*(RuntimeObject**)L_28 = L_33;
				Il2CppCodeGenWriteBarrier((void**)(RuntimeObject**)L_28, (void*)L_33);
				V_6 = (bool)1;
				goto IL_0101;
			}

IL_00ec_1:
			{
				goto IL_00f8;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_00f8:
	{
		RuntimeObject** L_34 = ___0_result;
		il2cpp_codegen_initobj(L_34, sizeof(RuntimeObject*));
		return (bool)0;
	}

IL_0101:
	{
		bool L_35 = V_6;
		return L_35;
	}
}
// Method Definition Index: 65456
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkStealingQueue_DangerousCopyTo_m94E502A92EB138838B97596A203E39F6E55FF508_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___0_array, int32_t ___1_arrayIndex, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	{
		int32_t L_0 = __this->____headIndex;
		il2cpp_codegen_memory_barrier();
		V_0 = L_0;
		int32_t L_1;
		L_1 = WorkStealingQueue_get_DangerousCount_m7EA23F3971CF379A3C0A87B49FECB670CD7D738E(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		V_1 = L_1;
		int32_t L_2 = ___1_arrayIndex;
		int32_t L_3 = V_1;
		V_2 = ((int32_t)il2cpp_codegen_subtract(((int32_t)il2cpp_codegen_add(L_2, L_3)), 1));
		goto IL_003e;
	}

IL_0018:
	{
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = ___0_array;
		int32_t L_5 = V_2;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_6 = __this->____array;
		il2cpp_codegen_memory_barrier();
		int32_t L_7 = V_0;
		int32_t L_8 = L_7;
		V_0 = ((int32_t)il2cpp_codegen_add(L_8, 1));
		int32_t L_9 = __this->____mask;
		il2cpp_codegen_memory_barrier();
		NullCheck(L_6);
		int32_t L_10 = ((int32_t)(L_8&L_9));
		RuntimeObject* L_11 = (L_6)->GetAt(static_cast<il2cpp_array_size_t>(L_10));
		NullCheck(L_4);
		(L_4)->SetAt(static_cast<il2cpp_array_size_t>(L_5), (RuntimeObject*)L_11);
		int32_t L_12 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_subtract(L_12, 1));
	}

IL_003e:
	{
		int32_t L_13 = V_2;
		int32_t L_14 = ___1_arrayIndex;
		if ((((int32_t)L_13) >= ((int32_t)L_14)))
		{
			goto IL_0018;
		}
	}
	{
		int32_t L_15 = V_1;
		return L_15;
	}
}
// Method Definition Index: 65457
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkStealingQueue_get_DangerousCount_m7EA23F3971CF379A3C0A87B49FECB670CD7D738E_gshared (WorkStealingQueue_t24446799B8DD68186AE6AC897ED0358FD1E03896* __this, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = __this->____addTakeCount;
		int32_t L_1 = __this->____stealCount;
		return ((int32_t)il2cpp_codegen_subtract(L_0, L_1));
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 65451
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkStealingQueue__ctor_mE0C0D6BEE2FB579DFAB968AEDB4BF1373A811EF1_gshared (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* __this, WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* ___0_nextQueue, const RuntimeMethod* method) 
{
	{
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_0 = (__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*)(__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*)SZArrayNew(il2cpp_rgctx_data(method->klass->rgctx_data, 0), (uint32_t)((int32_t)32));
		il2cpp_codegen_memory_barrier();
		__this->____array = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____array), (void*)L_0);
		il2cpp_codegen_memory_barrier();
		__this->____mask = ((int32_t)31);
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		int32_t L_1;
		L_1 = Environment_get_CurrentManagedThreadId_m66483AADCCC13272EBDCD94D31D2E52603C24BDF(NULL);
		__this->____ownerThreadId = L_1;
		WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* L_2 = ___0_nextQueue;
		__this->____nextQueue = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____nextQueue), (void*)L_2);
		return;
	}
}
// Method Definition Index: 65452
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkStealingQueue_LocalPush_m3DD3B524B0B5334F07A5826E44963BBA0FD6ABAD_gshared (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* __this, Il2CppFullySharedGenericAny ___0_item, int64_t* ___1_emptyToNonEmptyListTransitionCount, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3));
	const Il2CppFullySharedGenericAny L_25 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	const Il2CppFullySharedGenericAny L_57 = L_25;
	bool V_0 = false;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* V_3 = NULL;
	bool V_4 = false;
	int32_t V_5 = 0;
	__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* V_6 = NULL;
	int32_t V_7 = 0;
	{
		V_0 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_0203:
			{
				{
					il2cpp_codegen_memory_barrier();
					__this->____currentOp = 0;
					bool L_0 = V_0;
					if (!L_0)
					{
						goto IL_0215;
					}
				}
				{
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)__this, NULL);
				}

IL_0215:
				{
					return;
				}
			}
		});
		try
		{
			{
				int32_t* L_1 = (int32_t*)(&__this->____currentOp);
				il2cpp_codegen_memory_barrier();
				int32_t L_2;
				L_2 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_1, 1, NULL);
				int32_t L_3 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				V_1 = L_3;
				int32_t L_4 = V_1;
				if ((!(((uint32_t)L_4) == ((uint32_t)((int32_t)2147483647LL)))))
				{
					goto IL_007d_1;
				}
			}
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 0;
				V_3 = __this;
				V_4 = (bool)0;
			}
			{
				auto __finallyBlock = il2cpp::utils::Finally([&]
				{

FINALLY_0072_1:
					{
						{
							bool L_5 = V_4;
							if (!L_5)
							{
								goto IL_007c_1;
							}
						}
						{
							WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* L_6 = V_3;
							Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_6, NULL);
						}

IL_007c_1:
						{
							return;
						}
					}
				});
				try
				{
					WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* L_7 = V_3;
					Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_7, (&V_4), NULL);
					int32_t L_8 = __this->____headIndex;
					il2cpp_codegen_memory_barrier();
					int32_t L_9 = __this->____mask;
					il2cpp_codegen_memory_barrier();
					il2cpp_codegen_memory_barrier();
					__this->____headIndex = ((int32_t)(L_8&L_9));
					int32_t L_10 = V_1;
					int32_t L_11 = __this->____mask;
					il2cpp_codegen_memory_barrier();
					int32_t L_12 = ((int32_t)(L_10&L_11));
					V_1 = L_12;
					il2cpp_codegen_memory_barrier();
					__this->____tailIndex = L_12;
					int32_t* L_13 = (int32_t*)(&__this->____currentOp);
					il2cpp_codegen_memory_barrier();
					int32_t L_14;
					L_14 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_13, 1, NULL);
					goto IL_007d_1;
				}
				catch(Il2CppExceptionWrapper& e)
				{
					__finallyBlock.StoreException(e.ex);
				}
			}

IL_007d_1:
			{
				int32_t L_15 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				V_2 = L_15;
				bool L_16 = __this->____frozen;
				if (L_16)
				{
					goto IL_00cc_1;
				}
			}
			{
				int32_t L_17 = V_2;
				int32_t L_18 = V_1;
				int32_t L_19 = V_1;
				int32_t L_20 = V_2;
				int32_t L_21 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				if (!((int32_t)(((((int32_t)L_17) < ((int32_t)((int32_t)il2cpp_codegen_subtract(L_18, 1))))? 1 : 0)&((((int32_t)L_19) < ((int32_t)((int32_t)il2cpp_codegen_add(L_20, L_21))))? 1 : 0))))
				{
					goto IL_00cc_1;
				}
			}
			{
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_22 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_23 = V_1;
				int32_t L_24 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				il2cpp_codegen_memcpy(L_25, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3)) ? ___0_item : &___0_item), SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				NullCheck(L_22);
				il2cpp_codegen_memcpy((L_22)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_23&L_24)))), L_25, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(L_22)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_23&L_24)))), (void*)L_25);
				int32_t L_26 = V_1;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = ((int32_t)il2cpp_codegen_add(L_26, 1));
				goto IL_01f3_1;
			}

IL_00cc_1:
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)__this, (&V_0), NULL);
				int32_t L_27 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				V_2 = L_27;
				int32_t L_28 = V_1;
				int32_t L_29 = V_2;
				V_5 = ((int32_t)il2cpp_codegen_subtract(L_28, L_29));
				int32_t L_30 = V_5;
				int32_t L_31 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_30) < ((int32_t)L_31)))
				{
					goto IL_01ab_1;
				}
			}
			{
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_32 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_32);
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_33 = (__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*)(__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC*)SZArrayNew(il2cpp_rgctx_data(method->klass->rgctx_data, 0), (uint32_t)((int32_t)(((int32_t)(((RuntimeArray*)L_32)->max_length))<<1)));
				V_6 = L_33;
				int32_t L_34 = V_2;
				int32_t L_35 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_7 = ((int32_t)(L_34&L_35));
				int32_t L_36 = V_7;
				if (L_36)
				{
					goto IL_013a_1;
				}
			}
			{
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_37 = __this->____array;
				il2cpp_codegen_memory_barrier();
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_38 = V_6;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_39 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_39);
				Array_Copy_mB4904E17BD92E320613A3251C0205E0786B3BF41((RuntimeArray*)L_37, 0, (RuntimeArray*)L_38, 0, ((int32_t)(((RuntimeArray*)L_39)->max_length)), NULL);
				goto IL_0178_1;
			}

IL_013a_1:
			{
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_40 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_41 = V_7;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_42 = V_6;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_43 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_43);
				int32_t L_44 = V_7;
				Array_Copy_mB4904E17BD92E320613A3251C0205E0786B3BF41((RuntimeArray*)L_40, L_41, (RuntimeArray*)L_42, 0, ((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_43)->max_length)), L_44)), NULL);
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_45 = __this->____array;
				il2cpp_codegen_memory_barrier();
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_46 = V_6;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_47 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_47);
				int32_t L_48 = V_7;
				int32_t L_49 = V_7;
				Array_Copy_mB4904E17BD92E320613A3251C0205E0786B3BF41((RuntimeArray*)L_45, 0, (RuntimeArray*)L_46, ((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_47)->max_length)), L_48)), L_49, NULL);
			}

IL_0178_1:
			{
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_50 = V_6;
				il2cpp_codegen_memory_barrier();
				__this->____array = L_50;
				Il2CppCodeGenWriteBarrier((void**)(&__this->____array), (void*)L_50);
				il2cpp_codegen_memory_barrier();
				__this->____headIndex = 0;
				int32_t L_51 = V_5;
				int32_t L_52 = L_51;
				V_1 = L_52;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = L_52;
				int32_t L_53 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				il2cpp_codegen_memory_barrier();
				__this->____mask = ((int32_t)(((int32_t)(L_53<<1))|1));
			}

IL_01ab_1:
			{
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_54 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_55 = V_1;
				int32_t L_56 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				il2cpp_codegen_memcpy(L_57, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3)) ? ___0_item : &___0_item), SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				NullCheck(L_54);
				il2cpp_codegen_memcpy((L_54)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_55&L_56)))), L_57, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(L_54)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_55&L_56)))), (void*)L_57);
				int32_t L_58 = V_1;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = ((int32_t)il2cpp_codegen_add(L_58, 1));
				int32_t L_59 = V_5;
				if (L_59)
				{
					goto IL_01d9_1;
				}
			}
			{
				int64_t* L_60 = ___1_emptyToNonEmptyListTransitionCount;
				int64_t L_61;
				L_61 = Interlocked_Increment_m12CD643CCD5FCDE95DBC0AB07D09AF696E26F9C0(L_60, NULL);
			}

IL_01d9_1:
			{
				int32_t L_62 = __this->____addTakeCount;
				int32_t L_63 = __this->____stealCount;
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_subtract(L_62, L_63));
				__this->____stealCount = 0;
			}

IL_01f3_1:
			{
				int32_t L_64 = __this->____addTakeCount;
				if (((int64_t)L_64 + (int64_t)1 < (int64_t)kIl2CppInt32Min) || ((int64_t)L_64 + (int64_t)1 > (int64_t)kIl2CppInt32Max))
					IL2CPP_RAISE_MANAGED_EXCEPTION(il2cpp_codegen_get_overflow_exception(), method);
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_add(L_64, 1));
				goto IL_0216;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_0216:
	{
		return;
	}
}
// Method Definition Index: 65453
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WorkStealingQueue_LocalClear_m103E216AE3F28FB020776E261A1BDBB71FD9445C_gshared (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* __this, const RuntimeMethod* method) 
{
	WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* V_0 = NULL;
	bool V_1 = false;
	int32_t V_2 = 0;
	{
		V_0 = __this;
		V_1 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_005c:
			{
				{
					bool L_0 = V_1;
					if (!L_0)
					{
						goto IL_0065;
					}
				}
				{
					WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* L_1 = V_0;
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_1, NULL);
				}

IL_0065:
				{
					return;
				}
			}
		});
		try
		{
			{
				WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* L_2 = V_0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_2, (&V_1), NULL);
				int32_t L_3 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				int32_t L_4 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_3) >= ((int32_t)L_4)))
				{
					goto IL_005a_1;
				}
			}
			{
				int32_t L_5 = 0;
				V_2 = L_5;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = L_5;
				int32_t L_6 = V_2;
				il2cpp_codegen_memory_barrier();
				__this->____headIndex = L_6;
				int32_t L_7 = 0;
				V_2 = L_7;
				__this->____stealCount = L_7;
				int32_t L_8 = V_2;
				__this->____addTakeCount = L_8;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_9 = __this->____array;
				il2cpp_codegen_memory_barrier();
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_10 = __this->____array;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_10);
				Array_Clear_m50BAA3751899858B097D3FF2ED31F284703FE5CB((RuntimeArray*)L_9, 0, ((int32_t)(((RuntimeArray*)L_10)->max_length)), NULL);
			}

IL_005a_1:
			{
				goto IL_0066;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_0066:
	{
		return;
	}
}
// Method Definition Index: 65454
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WorkStealingQueue_TryLocalPop_m64D8230DEA88152F3874D56A0CDBB28CE92D0070_gshared (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* __this, Il2CppFullySharedGenericAny* ___0_result, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3));
	const Il2CppFullySharedGenericAny L_18 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	const Il2CppFullySharedGenericAny L_31 = L_18;
	const Il2CppFullySharedGenericAny L_21 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	const Il2CppFullySharedGenericAny L_34 = L_21;
	int32_t V_0 = 0;
	bool V_1 = false;
	int32_t V_2 = 0;
	Il2CppFullySharedGenericAny V_3 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	memset(V_3, 0, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	bool V_4 = false;
	int32_t V_5 = 0;
	{
		int32_t L_0 = __this->____tailIndex;
		il2cpp_codegen_memory_barrier();
		V_0 = L_0;
		int32_t L_1 = __this->____headIndex;
		il2cpp_codegen_memory_barrier();
		int32_t L_2 = V_0;
		if ((((int32_t)L_1) < ((int32_t)L_2)))
		{
			goto IL_001d;
		}
	}
	{
		Il2CppFullySharedGenericAny* L_3 = ___0_result;
		il2cpp_codegen_initobj(L_3, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
		return (bool)0;
	}

IL_001d:
	{
		V_1 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_0117:
			{
				{
					il2cpp_codegen_memory_barrier();
					__this->____currentOp = 0;
					bool L_4 = V_1;
					if (!L_4)
					{
						goto IL_0129;
					}
				}
				{
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)__this, NULL);
				}

IL_0129:
				{
					return;
				}
			}
		});
		try
		{
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 2;
				int32_t* L_5 = (int32_t*)(&__this->____tailIndex);
				il2cpp_codegen_memory_barrier();
				int32_t L_6 = V_0;
				int32_t L_7 = ((int32_t)il2cpp_codegen_subtract(L_6, 1));
				V_0 = L_7;
				int32_t L_8;
				L_8 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_5, L_7, NULL);
				bool L_9 = __this->____frozen;
				if (L_9)
				{
					goto IL_0098_1;
				}
			}
			{
				int32_t L_10 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				int32_t L_11 = V_0;
				if ((((int32_t)L_10) >= ((int32_t)L_11)))
				{
					goto IL_0098_1;
				}
			}
			{
				int32_t L_12 = V_0;
				int32_t L_13 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_2 = ((int32_t)(L_12&L_13));
				Il2CppFullySharedGenericAny* L_14 = ___0_result;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_15 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_16 = V_2;
				NullCheck(L_15);
				int32_t L_17 = L_16;
				il2cpp_codegen_memcpy(L_18, (L_15)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_17)), SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				il2cpp_codegen_memcpy((Il2CppFullySharedGenericAny*)L_14, L_18, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(Il2CppFullySharedGenericAny*)L_14, (void*)L_18);
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_19 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_20 = V_2;
				il2cpp_codegen_initobj((Il2CppFullySharedGenericAny*)V_3, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				il2cpp_codegen_memcpy(L_21, V_3, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				NullCheck(L_19);
				il2cpp_codegen_memcpy((L_19)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_20)), L_21, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(L_19)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_20)), (void*)L_21);
				int32_t L_22 = __this->____addTakeCount;
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_subtract(L_22, 1));
				V_4 = (bool)1;
				goto IL_012a;
			}

IL_0098_1:
			{
				il2cpp_codegen_memory_barrier();
				__this->____currentOp = 0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)__this, (&V_1), NULL);
				int32_t L_23 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				int32_t L_24 = V_0;
				if ((((int32_t)L_23) > ((int32_t)L_24)))
				{
					goto IL_0100_1;
				}
			}
			{
				int32_t L_25 = V_0;
				int32_t L_26 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_5 = ((int32_t)(L_25&L_26));
				Il2CppFullySharedGenericAny* L_27 = ___0_result;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_28 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_29 = V_5;
				NullCheck(L_28);
				int32_t L_30 = L_29;
				il2cpp_codegen_memcpy(L_31, (L_28)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_30)), SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				il2cpp_codegen_memcpy((Il2CppFullySharedGenericAny*)L_27, L_31, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(Il2CppFullySharedGenericAny*)L_27, (void*)L_31);
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_32 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_33 = V_5;
				il2cpp_codegen_initobj((Il2CppFullySharedGenericAny*)V_3, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				il2cpp_codegen_memcpy(L_34, V_3, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				NullCheck(L_32);
				il2cpp_codegen_memcpy((L_32)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_33)), L_34, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(L_32)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_33)), (void*)L_34);
				int32_t L_35 = __this->____addTakeCount;
				__this->____addTakeCount = ((int32_t)il2cpp_codegen_subtract(L_35, 1));
				V_4 = (bool)1;
				goto IL_012a;
			}

IL_0100_1:
			{
				int32_t L_36 = V_0;
				il2cpp_codegen_memory_barrier();
				__this->____tailIndex = ((int32_t)il2cpp_codegen_add(L_36, 1));
				Il2CppFullySharedGenericAny* L_37 = ___0_result;
				il2cpp_codegen_initobj(L_37, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				V_4 = (bool)0;
				goto IL_012a;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_012a:
	{
		bool L_38 = V_4;
		return L_38;
	}
}
// Method Definition Index: 65455
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool WorkStealingQueue_TrySteal_m789E536DF7E5E2A9CBFE77AF0519491A4C708551_gshared (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* __this, Il2CppFullySharedGenericAny* ___0_result, bool ___1_take, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	const uint32_t SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3));
	const Il2CppFullySharedGenericAny L_20 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	const Il2CppFullySharedGenericAny L_33 = L_20;
	const Il2CppFullySharedGenericAny L_23 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* V_0 = NULL;
	bool V_1 = false;
	int32_t V_2 = 0;
	SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675 V_3;
	memset((&V_3), 0, sizeof(V_3));
	int32_t V_4 = 0;
	Il2CppFullySharedGenericAny V_5 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	memset(V_5, 0, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	bool V_6 = false;
	{
		V_0 = __this;
		V_1 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_00ee:
			{
				{
					bool L_0 = V_1;
					if (!L_0)
					{
						goto IL_00f7;
					}
				}
				{
					WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* L_1 = V_0;
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_1, NULL);
				}

IL_00f7:
				{
					return;
				}
			}
		});
		try
		{
			{
				WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* L_2 = V_0;
				Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_2, (&V_1), NULL);
				int32_t L_3 = __this->____headIndex;
				il2cpp_codegen_memory_barrier();
				V_2 = L_3;
				bool L_4 = ___1_take;
				if (!L_4)
				{
					goto IL_00bf_1;
				}
			}
			{
				int32_t L_5 = V_2;
				int32_t L_6 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_5) >= ((int32_t)((int32_t)il2cpp_codegen_subtract(L_6, 1)))))
				{
					goto IL_004d_1;
				}
			}
			{
				int32_t L_7 = __this->____currentOp;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_7) == ((int32_t)1)))
				{
					goto IL_004d_1;
				}
			}
			{
				il2cpp_codegen_initobj((&V_3), sizeof(SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675));
			}

IL_003b_1:
			{
				il2cpp_codegen_runtime_class_init_inline(SpinWait_t51CFFA8FF70F1B430E075F96CFD936260D8CE675_il2cpp_TypeInfo_var);
				SpinWait_SpinOnce_m5B74E6B15013E90667646C0D943E886D4EC596AF((&V_3), NULL);
				int32_t L_8 = __this->____currentOp;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_8) == ((int32_t)1)))
				{
					goto IL_003b_1;
				}
			}

IL_004d_1:
			{
				int32_t* L_9 = (int32_t*)(&__this->____headIndex);
				il2cpp_codegen_memory_barrier();
				int32_t L_10 = V_2;
				int32_t L_11;
				L_11 = Interlocked_Exchange_m5810F59A9EF49D8003BA76D95398854254A091DF(L_9, ((int32_t)il2cpp_codegen_add(L_10, 1)), NULL);
				int32_t L_12 = V_2;
				int32_t L_13 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_12) >= ((int32_t)L_13)))
				{
					goto IL_00b4_1;
				}
			}
			{
				int32_t L_14 = V_2;
				int32_t L_15 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				V_4 = ((int32_t)(L_14&L_15));
				Il2CppFullySharedGenericAny* L_16 = ___0_result;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_17 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_18 = V_4;
				NullCheck(L_17);
				int32_t L_19 = L_18;
				il2cpp_codegen_memcpy(L_20, (L_17)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_19)), SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				il2cpp_codegen_memcpy((Il2CppFullySharedGenericAny*)L_16, L_20, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(Il2CppFullySharedGenericAny*)L_16, (void*)L_20);
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_21 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_22 = V_4;
				il2cpp_codegen_initobj((Il2CppFullySharedGenericAny*)V_5, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				il2cpp_codegen_memcpy(L_23, V_5, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				NullCheck(L_21);
				il2cpp_codegen_memcpy((L_21)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_22)), L_23, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(L_21)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_22)), (void*)L_23);
				int32_t L_24 = __this->____stealCount;
				__this->____stealCount = ((int32_t)il2cpp_codegen_add(L_24, 1));
				V_6 = (bool)1;
				goto IL_0101;
			}

IL_00b4_1:
			{
				int32_t L_25 = V_2;
				il2cpp_codegen_memory_barrier();
				__this->____headIndex = L_25;
				goto IL_00f8;
			}

IL_00bf_1:
			{
				int32_t L_26 = V_2;
				int32_t L_27 = __this->____tailIndex;
				il2cpp_codegen_memory_barrier();
				if ((((int32_t)L_26) >= ((int32_t)L_27)))
				{
					goto IL_00ec_1;
				}
			}
			{
				Il2CppFullySharedGenericAny* L_28 = ___0_result;
				__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_29 = __this->____array;
				il2cpp_codegen_memory_barrier();
				int32_t L_30 = V_2;
				int32_t L_31 = __this->____mask;
				il2cpp_codegen_memory_barrier();
				NullCheck(L_29);
				int32_t L_32 = ((int32_t)(L_30&L_31));
				il2cpp_codegen_memcpy(L_33, (L_29)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_32)), SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				il2cpp_codegen_memcpy((Il2CppFullySharedGenericAny*)L_28, L_33, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
				Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(Il2CppFullySharedGenericAny*)L_28, (void*)L_33);
				V_6 = (bool)1;
				goto IL_0101;
			}

IL_00ec_1:
			{
				goto IL_00f8;
			}
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_00f8:
	{
		Il2CppFullySharedGenericAny* L_34 = ___0_result;
		il2cpp_codegen_initobj(L_34, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
		return (bool)0;
	}

IL_0101:
	{
		bool L_35 = V_6;
		return L_35;
	}
}
// Method Definition Index: 65456
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkStealingQueue_DangerousCopyTo_m45F80B1FF0D38F1BE0A0315B1F48501D81284F06_gshared (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* __this, __Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* ___0_array, int32_t ___1_arrayIndex, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 3));
	const Il2CppFullySharedGenericAny L_11 = alloca(SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	{
		int32_t L_0 = __this->____headIndex;
		il2cpp_codegen_memory_barrier();
		V_0 = L_0;
		int32_t L_1;
		L_1 = ((  int32_t (*) (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 6)))(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		V_1 = L_1;
		int32_t L_2 = ___1_arrayIndex;
		int32_t L_3 = V_1;
		V_2 = ((int32_t)il2cpp_codegen_subtract(((int32_t)il2cpp_codegen_add(L_2, L_3)), 1));
		goto IL_003e;
	}

IL_0018:
	{
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_4 = ___0_array;
		int32_t L_5 = V_2;
		__Il2CppFullySharedGenericTypeU5BU5D_tCAB6D060972DD49223A834B7EEFEB9FE2D003BEC* L_6 = __this->____array;
		il2cpp_codegen_memory_barrier();
		int32_t L_7 = V_0;
		int32_t L_8 = L_7;
		V_0 = ((int32_t)il2cpp_codegen_add(L_8, 1));
		int32_t L_9 = __this->____mask;
		il2cpp_codegen_memory_barrier();
		NullCheck(L_6);
		int32_t L_10 = ((int32_t)(L_8&L_9));
		il2cpp_codegen_memcpy(L_11, (L_6)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_10)), SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
		NullCheck(L_4);
		il2cpp_codegen_memcpy((L_4)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_5)), L_11, SizeOf_T_t17EBD739EED235CAB9EDAFF648A720C8D45BCBF1);
		Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 3), (void**)(L_4)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_5)), (void*)L_11);
		int32_t L_12 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_subtract(L_12, 1));
	}

IL_003e:
	{
		int32_t L_13 = V_2;
		int32_t L_14 = ___1_arrayIndex;
		if ((((int32_t)L_13) >= ((int32_t)L_14)))
		{
			goto IL_0018;
		}
	}
	{
		int32_t L_15 = V_1;
		return L_15;
	}
}
// Method Definition Index: 65457
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t WorkStealingQueue_get_DangerousCount_m1BC6D29975BB205B49F508FF41A9C4AE59E0F6FA_gshared (WorkStealingQueue_t22AD7C39FB7ED4674B33265814255B725DA76AC3* __this, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = __this->____addTakeCount;
		int32_t L_1 = __this->____stealCount;
		return ((int32_t)il2cpp_codegen_subtract(L_0, L_1));
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 53951
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Wrapper__ctor_m42F8525ADAACA078DEB51D9C69EB7ABD7E3ACA5D_gshared (Wrapper_tDCD6E9769A559E52C6D10132E041EA3EFF52BC98* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_mE86A5384298CF3DF2344D386DC4CA589E42DE399_Multicast(WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234* __this, Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234* currentDelegate = reinterpret_cast<WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D*, float, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_mE86A5384298CF3DF2344D386DC4CA589E42DE399_OpenInst(WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234* __this, Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
void WriteDelegate_Invoke_mE86A5384298CF3DF2344D386DC4CA589E42DE399_OpenStatic(WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234* __this, Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_m630834333CB7AE51AB01F7713157EE57991D03D7_gshared (WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_mE86A5384298CF3DF2344D386DC4CA589E42DE399_OpenStatic;
		else
			{
				__this->___invoke_impl = __this->___method_ptr;
				__this->___method_code = (intptr_t)__this->___m_target;
			}
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_mE86A5384298CF3DF2344D386DC4CA589E42DE399_OpenInst;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = __this->___method_ptr;
			__this->___method_code = (intptr_t)__this->___m_target;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_mE86A5384298CF3DF2344D386DC4CA589E42DE399_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_mE86A5384298CF3DF2344D386DC4CA589E42DE399_gshared (WriteDelegate_t6A0EB21668C92D873ACB875924DE54518D8B7234* __this, Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D* ___0_val, float ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, Rect_tA04E0F8A1830E767F40FB27ECD8D309303571F0D*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_m86BA7048E52732500448764B50B058BBB1AB029A_Multicast(WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1* __this, RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1* currentDelegate = reinterpret_cast<WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8*, int32_t, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_m86BA7048E52732500448764B50B058BBB1AB029A_OpenInst(WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1* __this, RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
void WriteDelegate_Invoke_m86BA7048E52732500448764B50B058BBB1AB029A_OpenStatic(WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1* __this, RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_m2AA9BE88F67A2DA68720D6B5F2AFFA89E23BB597_gshared (WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m86BA7048E52732500448764B50B058BBB1AB029A_OpenStatic;
		else
			{
				__this->___invoke_impl = __this->___method_ptr;
				__this->___method_code = (intptr_t)__this->___m_target;
			}
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m86BA7048E52732500448764B50B058BBB1AB029A_OpenInst;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = __this->___method_ptr;
			__this->___method_code = (intptr_t)__this->___m_target;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_m86BA7048E52732500448764B50B058BBB1AB029A_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_m86BA7048E52732500448764B50B058BBB1AB029A_gshared (WriteDelegate_t737D0DA8D96562AD76BB67D9C5A5199EDB0171E1* __this, RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, RectInt_t1744D10E1063135DA9D574F95205B98DAC600CB8*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_m6DAF352FCBE9E4A508A3AB7056209E9DEDCE6ACC_Multicast(WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D* __this, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D* currentDelegate = reinterpret_cast<WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7*, float, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_m6DAF352FCBE9E4A508A3AB7056209E9DEDCE6ACC_OpenInst(WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D* __this, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
void WriteDelegate_Invoke_m6DAF352FCBE9E4A508A3AB7056209E9DEDCE6ACC_OpenStatic(WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D* __this, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_m4F33ED7C62B9E61B33CC81A7B5C739D0396BC209_gshared (WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m6DAF352FCBE9E4A508A3AB7056209E9DEDCE6ACC_OpenStatic;
		else
			{
				__this->___invoke_impl = __this->___method_ptr;
				__this->___method_code = (intptr_t)__this->___m_target;
			}
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m6DAF352FCBE9E4A508A3AB7056209E9DEDCE6ACC_OpenInst;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = __this->___method_ptr;
			__this->___method_code = (intptr_t)__this->___m_target;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_m6DAF352FCBE9E4A508A3AB7056209E9DEDCE6ACC_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_m6DAF352FCBE9E4A508A3AB7056209E9DEDCE6ACC_gshared (WriteDelegate_t907C2DAF3A619B832C6D9441AF1C4CDA7914AF1D* __this, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7* ___0_val, float ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, Vector2_t1FD6F485C871E832B347AB2DC8CBA08B739D8DF7*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_mDCBB3FCC716BDB01FBB6A93055CB254FEFF466CA_Multicast(WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7* __this, Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7* currentDelegate = reinterpret_cast<WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A*, int32_t, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_mDCBB3FCC716BDB01FBB6A93055CB254FEFF466CA_OpenInst(WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7* __this, Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
void WriteDelegate_Invoke_mDCBB3FCC716BDB01FBB6A93055CB254FEFF466CA_OpenStatic(WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7* __this, Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_m1067E857FEE330354C23F3DB8C7B30143BCA679D_gshared (WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_mDCBB3FCC716BDB01FBB6A93055CB254FEFF466CA_OpenStatic;
		else
			{
				__this->___invoke_impl = __this->___method_ptr;
				__this->___method_code = (intptr_t)__this->___m_target;
			}
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_mDCBB3FCC716BDB01FBB6A93055CB254FEFF466CA_OpenInst;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = __this->___method_ptr;
			__this->___method_code = (intptr_t)__this->___m_target;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_mDCBB3FCC716BDB01FBB6A93055CB254FEFF466CA_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_mDCBB3FCC716BDB01FBB6A93055CB254FEFF466CA_gshared (WriteDelegate_tE9F20B5E1A664D239EFDD91143B963A216AAB1C7* __this, Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, Vector2Int_t69B2886EBAB732D9B880565E18E7568F3DE0CE6A*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_m78604525C7D756C919B71D80FE8B7C42684A0F60_Multicast(WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789* __this, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789* currentDelegate = reinterpret_cast<WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2*, float, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_m78604525C7D756C919B71D80FE8B7C42684A0F60_OpenInst(WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789* __this, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
void WriteDelegate_Invoke_m78604525C7D756C919B71D80FE8B7C42684A0F60_OpenStatic(WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789* __this, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_mC9073752A9F993A89037EB0FCF8968BCB14AE3A3_gshared (WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m78604525C7D756C919B71D80FE8B7C42684A0F60_OpenStatic;
		else
			{
				__this->___invoke_impl = __this->___method_ptr;
				__this->___method_code = (intptr_t)__this->___m_target;
			}
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m78604525C7D756C919B71D80FE8B7C42684A0F60_OpenInst;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = __this->___method_ptr;
			__this->___method_code = (intptr_t)__this->___m_target;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_m78604525C7D756C919B71D80FE8B7C42684A0F60_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_m78604525C7D756C919B71D80FE8B7C42684A0F60_gshared (WriteDelegate_t023838BBDB83C818E9FDC98ABFA9A9396823E789* __this, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2* ___0_val, float ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, Vector3_t24C512C7B96BBABAD472002D0BA2BDA40A5A80B2*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_m86B402A7CEAB3ADDAB046D1975023F53BFB4E878_Multicast(WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0* __this, Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0* currentDelegate = reinterpret_cast<WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376*, int32_t, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_m86B402A7CEAB3ADDAB046D1975023F53BFB4E878_OpenInst(WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0* __this, Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
void WriteDelegate_Invoke_m86B402A7CEAB3ADDAB046D1975023F53BFB4E878_OpenStatic(WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0* __this, Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_mAA1CB4066F7346F24DA38ED5A1240309F0C4BC60_gshared (WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m86B402A7CEAB3ADDAB046D1975023F53BFB4E878_OpenStatic;
		else
			{
				__this->___invoke_impl = __this->___method_ptr;
				__this->___method_code = (intptr_t)__this->___m_target;
			}
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m86B402A7CEAB3ADDAB046D1975023F53BFB4E878_OpenInst;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = __this->___method_ptr;
			__this->___method_code = (intptr_t)__this->___m_target;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_m86B402A7CEAB3ADDAB046D1975023F53BFB4E878_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_m86B402A7CEAB3ADDAB046D1975023F53BFB4E878_gshared (WriteDelegate_t98EF2EAAD5827503FF2F0213C32A4961968D75A0* __this, Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376* ___0_val, int32_t ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, Vector3Int_t65CB06F557251D18A37BD71F3655BA836A357376*, int32_t, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_mCB317932D29E39DF2768AD06B5DA7A84CD3A2C16_Multicast(WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8* __this, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8* currentDelegate = reinterpret_cast<WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3*, float, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_mCB317932D29E39DF2768AD06B5DA7A84CD3A2C16_OpenInst(WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8* __this, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
void WriteDelegate_Invoke_mCB317932D29E39DF2768AD06B5DA7A84CD3A2C16_OpenStatic(WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8* __this, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3* ___0_val, float ___1_fieldValue, const RuntimeMethod* method)
{
	typedef void (*FunctionPointerType) (Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___method_ptr)(___0_val, ___1_fieldValue, method);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_m6165AFE856EC0CC9CDE8A145B404312481261696_gshared (WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_mCB317932D29E39DF2768AD06B5DA7A84CD3A2C16_OpenStatic;
		else
			{
				__this->___invoke_impl = __this->___method_ptr;
				__this->___method_code = (intptr_t)__this->___m_target;
			}
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_mCB317932D29E39DF2768AD06B5DA7A84CD3A2C16_OpenInst;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = __this->___method_ptr;
			__this->___method_code = (intptr_t)__this->___m_target;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_mCB317932D29E39DF2768AD06B5DA7A84CD3A2C16_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_mCB317932D29E39DF2768AD06B5DA7A84CD3A2C16_gshared (WriteDelegate_t055D02BADD3A5DFA009116D80BC93C235313CAE8* __this, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3* ___0_val, float ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, Vector4_t58B63D32F48C0DBF50DE2C60794C4676C80EDBE3*, float, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
void WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_Multicast(WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* __this, Il2CppFullySharedGenericAny* ___0_val, Il2CppFullySharedGenericAny ___1_fieldValue, const RuntimeMethod* method)
{
	il2cpp_array_size_t length = __this->___delegates->max_length;
	Delegate_t** delegatesToInvoke = reinterpret_cast<Delegate_t**>(__this->___delegates->GetAddressAtUnchecked(0));
	for (il2cpp_array_size_t i = 0; i < length; i++)
	{
		WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* currentDelegate = reinterpret_cast<WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7*>(delegatesToInvoke[i]);
		typedef void (*FunctionPointerType) (RuntimeObject*, Il2CppFullySharedGenericAny*, Il2CppFullySharedGenericAny, const RuntimeMethod*);
		((FunctionPointerType)currentDelegate->___invoke_impl)((Il2CppObject*)currentDelegate->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(currentDelegate->___method));
	}
}
void WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_OpenStaticInvoker(WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* __this, Il2CppFullySharedGenericAny* ___0_val, Il2CppFullySharedGenericAny ___1_fieldValue, const RuntimeMethod* method)
{
	InvokerActionInvoker2< Il2CppFullySharedGenericAny*, Il2CppFullySharedGenericAny >::Invoke((Il2CppMethodPointer)__this->___method_ptr, method, NULL, ___0_val, ___1_fieldValue);
}
void WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_ClosedStaticInvoker(WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* __this, Il2CppFullySharedGenericAny* ___0_val, Il2CppFullySharedGenericAny ___1_fieldValue, const RuntimeMethod* method)
{
	InvokerActionInvoker3< RuntimeObject*, Il2CppFullySharedGenericAny*, Il2CppFullySharedGenericAny >::Invoke((Il2CppMethodPointer)__this->___method_ptr, method, NULL, __this->___m_target, ___0_val, ___1_fieldValue);
}
void WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_ClosedInstInvoker(WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* __this, Il2CppFullySharedGenericAny* ___0_val, Il2CppFullySharedGenericAny ___1_fieldValue, const RuntimeMethod* method)
{
	InvokerActionInvoker2< Il2CppFullySharedGenericAny*, Il2CppFullySharedGenericAny >::Invoke((Il2CppMethodPointer)__this->___method_ptr, method, __this->___m_target, ___0_val, ___1_fieldValue);
}
void WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_OpenInstInvoker(WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* __this, Il2CppFullySharedGenericAny* ___0_val, Il2CppFullySharedGenericAny ___1_fieldValue, const RuntimeMethod* method)
{
	NullCheck(___0_val);
	InvokerActionInvoker1< Il2CppFullySharedGenericAny >::Invoke((Il2CppMethodPointer)__this->___method_ptr, method, ___0_val, ___1_fieldValue);
}
// Method Definition Index: 14057
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate__ctor_mCE6F306923A685DD2E41E1BAABB666E0F7B4E137_gshared (WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) 
{
	__this->___method_ptr = (intptr_t)il2cpp_codegen_get_method_pointer((RuntimeMethod*)___1_method);
	__this->___method = ___1_method;
	__this->___m_target = ___0_object;
	Il2CppCodeGenWriteBarrier((void**)(&__this->___m_target), (void*)___0_object);
	int parameterCount = il2cpp_codegen_method_parameter_count((RuntimeMethod*)___1_method);
	__this->___method_code = (intptr_t)__this;
	if (MethodIsStatic((RuntimeMethod*)___1_method))
	{
		bool isOpen = parameterCount == 2;
		if (isOpen)
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_OpenStaticInvoker;
		else
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_ClosedStaticInvoker;
	}
	else
	{
		bool isOpen = parameterCount == 1;
		if (isOpen)
		{
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_OpenInstInvoker;
		}
		else
		{
			if (___0_object == NULL)
				il2cpp_codegen_raise_exception(il2cpp_codegen_get_argument_exception(NULL, "Delegate to an instance method cannot have null 'this'."), NULL);
			__this->___invoke_impl = (intptr_t)&WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_ClosedInstInvoker;
		}
	}
	__this->___extra_arg = (intptr_t)&WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_Multicast;
}
// Method Definition Index: 14058
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void WriteDelegate_Invoke_m314AB1F8889F277917B21F5F09093D797490FE93_gshared (WriteDelegate_tCC7EDE8329D3D4B81ABF643CABCC600B2CC335D7* __this, Il2CppFullySharedGenericAny* ___0_val, Il2CppFullySharedGenericAny ___1_fieldValue, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, Il2CppFullySharedGenericAny*, Il2CppFullySharedGenericAny, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_val, ___1_fieldValue, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 141127
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XHashtableState__ctor_m37E975C296D7D8CDF9ACAA9ABD7C4A0C82C427D3_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* ___0_extractKey, int32_t ___1_capacity, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		int32_t L_0 = ___1_capacity;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_1 = (Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C*)(Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C*)SZArrayNew(Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C_il2cpp_TypeInfo_var, (uint32_t)L_0);
		__this->____buckets = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____buckets), (void*)L_1);
		int32_t L_2 = ___1_capacity;
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_3 = (EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635*)(EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635*)SZArrayNew(il2cpp_rgctx_data(method->klass->rgctx_data, 1), (uint32_t)L_2);
		__this->____entries = L_3;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____entries), (void*)L_3);
		ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* L_4 = ___0_extractKey;
		__this->____extractKey = L_4;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____extractKey), (void*)L_4);
		return;
	}
}
// Method Definition Index: 141128
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* XHashtableState_Resize_mDB3BA5C327EF6A86F6CE58B59E1D7430E62532C5_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* V_1 = NULL;
	int32_t V_2 = 0;
	int32_t V_3 = 0;
	int32_t V_4 = 0;
	int32_t V_5 = 0;
	RuntimeObject* V_6 = NULL;
	{
		int32_t L_0 = __this->____numEntries;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_1 = __this->____buckets;
		NullCheck(L_1);
		if ((((int32_t)L_0) >= ((int32_t)((int32_t)(((RuntimeArray*)L_1)->max_length)))))
		{
			goto IL_0012;
		}
	}
	{
		return __this;
	}

IL_0012:
	{
		V_0 = 0;
		V_2 = 0;
		goto IL_00a7;
	}

IL_001b:
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_2 = __this->____buckets;
		int32_t L_3 = V_2;
		NullCheck(L_2);
		int32_t L_4 = L_3;
		int32_t L_5 = (L_2)->GetAt(static_cast<il2cpp_array_size_t>(L_4));
		V_3 = L_5;
		int32_t L_6 = V_3;
		if (L_6)
		{
			goto IL_009f;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_7 = __this->____buckets;
		int32_t L_8 = V_2;
		NullCheck(L_7);
		int32_t L_9;
		L_9 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717(((L_7)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_8))), (-1), 0, NULL);
		V_3 = L_9;
		goto IL_009f;
	}

IL_003d:
	{
		ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* L_10 = __this->____extractKey;
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_11 = __this->____entries;
		int32_t L_12 = V_3;
		NullCheck(L_11);
		RuntimeObject* L_13 = ((L_11)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_12)))->___Value;
		NullCheck(L_10);
		String_t* L_14;
		L_14 = ExtractKeyDelegate_Invoke_m3214A3F92C2EB22088AB7738E8DFE1BE70925148_inline(L_10, L_13, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		if (!L_14)
		{
			goto IL_005f;
		}
	}
	{
		int32_t L_15 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_add(L_15, 1));
	}

IL_005f:
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_16 = __this->____entries;
		int32_t L_17 = V_3;
		NullCheck(L_16);
		int32_t L_18 = ((L_16)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_17)))->___Next;
		if (L_18)
		{
			goto IL_008d;
		}
	}
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_19 = __this->____entries;
		int32_t L_20 = V_3;
		NullCheck(L_19);
		int32_t* L_21 = (int32_t*)(&((L_19)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_20)))->___Next);
		int32_t L_22;
		L_22 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717(L_21, (-1), 0, NULL);
		V_3 = L_22;
		goto IL_009f;
	}

IL_008d:
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_23 = __this->____entries;
		int32_t L_24 = V_3;
		NullCheck(L_23);
		int32_t L_25 = ((L_23)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_24)))->___Next;
		V_3 = L_25;
	}

IL_009f:
	{
		int32_t L_26 = V_3;
		if ((((int32_t)L_26) > ((int32_t)0)))
		{
			goto IL_003d;
		}
	}
	{
		int32_t L_27 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_27, 1));
	}

IL_00a7:
	{
		int32_t L_28 = V_2;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_29 = __this->____buckets;
		NullCheck(L_29);
		if ((((int32_t)L_28) < ((int32_t)((int32_t)(((RuntimeArray*)L_29)->max_length)))))
		{
			goto IL_001b;
		}
	}
	{
		int32_t L_30 = V_0;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_31 = __this->____buckets;
		NullCheck(L_31);
		if ((((int32_t)L_30) >= ((int32_t)((int32_t)(((int32_t)(((RuntimeArray*)L_31)->max_length))/2)))))
		{
			goto IL_00cd;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_32 = __this->____buckets;
		NullCheck(L_32);
		V_0 = ((int32_t)(((RuntimeArray*)L_32)->max_length));
		goto IL_00e2;
	}

IL_00cd:
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_33 = __this->____buckets;
		NullCheck(L_33);
		V_0 = ((int32_t)il2cpp_codegen_multiply(((int32_t)(((RuntimeArray*)L_33)->max_length)), 2));
		int32_t L_34 = V_0;
		if ((((int32_t)L_34) >= ((int32_t)0)))
		{
			goto IL_00e2;
		}
	}
	{
		OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C* L_35 = (OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C*)il2cpp_codegen_object_new(((RuntimeClass*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C_il2cpp_TypeInfo_var)));
		OverflowException__ctor_m7F6A928C9BE47384586BDDE8B4B87666421E0F1A(L_35, NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_35, method);
	}

IL_00e2:
	{
		ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* L_36 = __this->____extractKey;
		int32_t L_37 = V_0;
		XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_38 = (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 0));
		XHashtableState__ctor_m37E975C296D7D8CDF9ACAA9ABD7C4A0C82C427D3(L_38, L_36, L_37, il2cpp_rgctx_method(method->klass->rgctx_data, 7));
		V_1 = L_38;
		V_4 = 0;
		goto IL_013b;
	}

IL_00f4:
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_39 = __this->____buckets;
		int32_t L_40 = V_4;
		NullCheck(L_39);
		int32_t L_41 = L_40;
		int32_t L_42 = (L_39)->GetAt(static_cast<il2cpp_array_size_t>(L_41));
		V_5 = L_42;
		goto IL_0130;
	}

IL_0101:
	{
		XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_43 = V_1;
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_44 = __this->____entries;
		int32_t L_45 = V_5;
		NullCheck(L_44);
		RuntimeObject* L_46 = ((L_44)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_45)))->___Value;
		NullCheck(L_43);
		bool L_47;
		L_47 = XHashtableState_TryAdd_m951A9A914307495745E2B33DB0BA6583FB4060A1(L_43, L_46, (&V_6), il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_48 = __this->____entries;
		int32_t L_49 = V_5;
		NullCheck(L_48);
		int32_t L_50 = ((L_48)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_49)))->___Next;
		V_5 = L_50;
	}

IL_0130:
	{
		int32_t L_51 = V_5;
		if ((((int32_t)L_51) > ((int32_t)0)))
		{
			goto IL_0101;
		}
	}
	{
		int32_t L_52 = V_4;
		V_4 = ((int32_t)il2cpp_codegen_add(L_52, 1));
	}

IL_013b:
	{
		int32_t L_53 = V_4;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_54 = __this->____buckets;
		NullCheck(L_54);
		if ((((int32_t)L_53) < ((int32_t)((int32_t)(((RuntimeArray*)L_54)->max_length)))))
		{
			goto IL_00f4;
		}
	}
	{
		XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_55 = V_1;
		return L_55;
	}
}
// Method Definition Index: 141129
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_TryGetValue_m22BFB31B1A4E2CEDE4CA7133F4A0B50B9E514308_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, RuntimeObject** ___3_value, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	{
		String_t* L_0 = ___0_key;
		int32_t L_1 = ___1_index;
		int32_t L_2 = ___2_count;
		int32_t L_3;
		L_3 = XHashtableState_ComputeHashCode_mD58A823B4A09F8DB93853EA9536E6B457B59F1B1(L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_0 = L_3;
		V_1 = 0;
		int32_t L_4 = V_0;
		String_t* L_5 = ___0_key;
		int32_t L_6 = ___1_index;
		int32_t L_7 = ___2_count;
		bool L_8;
		L_8 = XHashtableState_FindEntry_m3AC0430564190D3FCAD9A8DB1DD0E725F7392A91(__this, L_4, L_5, L_6, L_7, (&V_1), il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		if (!L_8)
		{
			goto IL_0033;
		}
	}
	{
		RuntimeObject** L_9 = ___3_value;
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_10 = __this->____entries;
		int32_t L_11 = V_1;
		NullCheck(L_10);
		RuntimeObject* L_12 = ((L_10)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_11)))->___Value;
		*(RuntimeObject**)L_9 = L_12;
		Il2CppCodeGenWriteBarrier((void**)(RuntimeObject**)L_9, (void*)L_12);
		return (bool)1;
	}

IL_0033:
	{
		RuntimeObject** L_13 = ___3_value;
		il2cpp_codegen_initobj(L_13, sizeof(RuntimeObject*));
		return (bool)0;
	}
}
// Method Definition Index: 141130
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_TryAdd_m951A9A914307495745E2B33DB0BA6583FB4060A1_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, RuntimeObject* ___0_value, RuntimeObject** ___1_newValue, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	String_t* V_2 = NULL;
	int32_t V_3 = 0;
	{
		RuntimeObject** L_0 = ___1_newValue;
		RuntimeObject* L_1 = ___0_value;
		*(RuntimeObject**)L_0 = L_1;
		Il2CppCodeGenWriteBarrier((void**)(RuntimeObject**)L_0, (void*)L_1);
		ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* L_2 = __this->____extractKey;
		RuntimeObject* L_3 = ___0_value;
		NullCheck(L_2);
		String_t* L_4;
		L_4 = ExtractKeyDelegate_Invoke_m3214A3F92C2EB22088AB7738E8DFE1BE70925148_inline(L_2, L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		V_2 = L_4;
		String_t* L_5 = V_2;
		if (L_5)
		{
			goto IL_0019;
		}
	}
	{
		return (bool)1;
	}

IL_0019:
	{
		String_t* L_6 = V_2;
		String_t* L_7 = V_2;
		NullCheck(L_7);
		int32_t L_8;
		L_8 = String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline(L_7, NULL);
		int32_t L_9;
		L_9 = XHashtableState_ComputeHashCode_mD58A823B4A09F8DB93853EA9536E6B457B59F1B1(L_6, 0, L_8, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_3 = L_9;
		int32_t* L_10 = (int32_t*)(&__this->____numEntries);
		int32_t L_11;
		L_11 = Interlocked_Increment_m3C240C32E8D9544EC050B74D4F28EEB58F1F9309(L_10, NULL);
		V_0 = L_11;
		int32_t L_12 = V_0;
		if ((((int32_t)L_12) < ((int32_t)0)))
		{
			goto IL_0042;
		}
	}
	{
		int32_t L_13 = V_0;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_14 = __this->____buckets;
		NullCheck(L_14);
		if ((((int32_t)L_13) < ((int32_t)((int32_t)(((RuntimeArray*)L_14)->max_length)))))
		{
			goto IL_0044;
		}
	}

IL_0042:
	{
		return (bool)0;
	}

IL_0044:
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_15 = __this->____entries;
		int32_t L_16 = V_0;
		NullCheck(L_15);
		RuntimeObject* L_17 = ___0_value;
		((L_15)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_16)))->___Value = L_17;
		Il2CppCodeGenWriteBarrier((void**)(&((L_15)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_16)))->___Value), (void*)L_17);
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_18 = __this->____entries;
		int32_t L_19 = V_0;
		NullCheck(L_18);
		int32_t L_20 = V_3;
		((L_18)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_19)))->___HashCode = L_20;
		Thread_MemoryBarrier_m83873F1E6CEB16C0781941141382DA874A36097D(NULL);
		V_1 = 0;
		goto IL_00b7;
	}

IL_0071:
	{
		int32_t L_21 = V_1;
		if (L_21)
		{
			goto IL_0095;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_22 = __this->____buckets;
		int32_t L_23 = V_3;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_24 = __this->____buckets;
		NullCheck(L_24);
		NullCheck(L_22);
		int32_t L_25 = V_0;
		int32_t L_26;
		L_26 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717(((L_22)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_23&((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_24)->max_length)), 1))))))), L_25, 0, NULL);
		V_1 = L_26;
		goto IL_00ae;
	}

IL_0095:
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_27 = __this->____entries;
		int32_t L_28 = V_1;
		NullCheck(L_27);
		int32_t* L_29 = (int32_t*)(&((L_27)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_28)))->___Next);
		int32_t L_30 = V_0;
		int32_t L_31;
		L_31 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717(L_29, L_30, 0, NULL);
		V_1 = L_31;
	}

IL_00ae:
	{
		int32_t L_32 = V_1;
		if ((((int32_t)L_32) > ((int32_t)0)))
		{
			goto IL_00b7;
		}
	}
	{
		int32_t L_33 = V_1;
		return (bool)((((int32_t)L_33) == ((int32_t)0))? 1 : 0);
	}

IL_00b7:
	{
		int32_t L_34 = V_3;
		String_t* L_35 = V_2;
		String_t* L_36 = V_2;
		NullCheck(L_36);
		int32_t L_37;
		L_37 = String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline(L_36, NULL);
		bool L_38;
		L_38 = XHashtableState_FindEntry_m3AC0430564190D3FCAD9A8DB1DD0E725F7392A91(__this, L_34, L_35, 0, L_37, (&V_1), il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		if (!L_38)
		{
			goto IL_0071;
		}
	}
	{
		RuntimeObject** L_39 = ___1_newValue;
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_40 = __this->____entries;
		int32_t L_41 = V_1;
		NullCheck(L_40);
		RuntimeObject* L_42 = ((L_40)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_41)))->___Value;
		*(RuntimeObject**)L_39 = L_42;
		Il2CppCodeGenWriteBarrier((void**)(RuntimeObject**)L_39, (void*)L_42);
		return (bool)1;
	}
}
// Method Definition Index: 141131
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_FindEntry_m3AC0430564190D3FCAD9A8DB1DD0E725F7392A91_gshared (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* __this, int32_t ___0_hashCode, String_t* ___1_key, int32_t ___2_index, int32_t ___3_count, int32_t* ___4_entryIndex, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	String_t* V_2 = NULL;
	{
		int32_t* L_0 = ___4_entryIndex;
		int32_t L_1 = *((int32_t*)L_0);
		V_0 = L_1;
		int32_t L_2 = V_0;
		if (L_2)
		{
			goto IL_0020;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_3 = __this->____buckets;
		int32_t L_4 = ___0_hashCode;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_5 = __this->____buckets;
		NullCheck(L_5);
		NullCheck(L_3);
		int32_t L_6 = ((int32_t)(L_4&((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_5)->max_length)), 1))));
		int32_t L_7 = (L_3)->GetAt(static_cast<il2cpp_array_size_t>(L_6));
		V_1 = L_7;
		goto IL_00f9;
	}

IL_0020:
	{
		int32_t L_8 = V_0;
		V_1 = L_8;
		goto IL_00f9;
	}

IL_0027:
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_9 = __this->____entries;
		int32_t L_10 = V_1;
		NullCheck(L_9);
		int32_t L_11 = ((L_9)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_10)))->___HashCode;
		int32_t L_12 = ___0_hashCode;
		if ((!(((uint32_t)L_11) == ((uint32_t)L_12))))
		{
			goto IL_00e5;
		}
	}
	{
		ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* L_13 = __this->____extractKey;
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_14 = __this->____entries;
		int32_t L_15 = V_1;
		NullCheck(L_14);
		RuntimeObject* L_16 = ((L_14)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_15)))->___Value;
		NullCheck(L_13);
		String_t* L_17;
		L_17 = ExtractKeyDelegate_Invoke_m3214A3F92C2EB22088AB7738E8DFE1BE70925148_inline(L_13, L_16, il2cpp_rgctx_method(method->klass->rgctx_data, 6));
		V_2 = L_17;
		String_t* L_18 = V_2;
		if (L_18)
		{
			goto IL_00c8;
		}
	}
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_19 = __this->____entries;
		int32_t L_20 = V_1;
		NullCheck(L_19);
		int32_t L_21 = ((L_19)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_20)))->___Next;
		if ((((int32_t)L_21) <= ((int32_t)0)))
		{
			goto IL_00e5;
		}
	}
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_22 = __this->____entries;
		int32_t L_23 = V_1;
		NullCheck(L_22);
		RuntimeObject** L_24 = (RuntimeObject**)(&((L_22)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_23)))->___Value);
		il2cpp_codegen_initobj(L_24, sizeof(RuntimeObject*));
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_25 = __this->____entries;
		int32_t L_26 = V_1;
		NullCheck(L_25);
		int32_t L_27 = ((L_25)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_26)))->___Next;
		V_1 = L_27;
		int32_t L_28 = V_0;
		if (L_28)
		{
			goto IL_00b4;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_29 = __this->____buckets;
		int32_t L_30 = ___0_hashCode;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_31 = __this->____buckets;
		NullCheck(L_31);
		int32_t L_32 = V_1;
		NullCheck(L_29);
		(L_29)->SetAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_30&((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_31)->max_length)), 1))))), (int32_t)L_32);
		goto IL_00f9;
	}

IL_00b4:
	{
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_33 = __this->____entries;
		int32_t L_34 = V_0;
		NullCheck(L_33);
		int32_t L_35 = V_1;
		((L_33)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_34)))->___Next = L_35;
		goto IL_00f9;
	}

IL_00c8:
	{
		int32_t L_36 = ___3_count;
		String_t* L_37 = V_2;
		NullCheck(L_37);
		int32_t L_38;
		L_38 = String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline(L_37, NULL);
		if ((!(((uint32_t)L_36) == ((uint32_t)L_38))))
		{
			goto IL_00e5;
		}
	}
	{
		String_t* L_39 = ___1_key;
		int32_t L_40 = ___2_index;
		String_t* L_41 = V_2;
		int32_t L_42 = ___3_count;
		int32_t L_43;
		L_43 = String_CompareOrdinal_m8940CFAE90021ED8DA3F2DF8226941C9EEB2E32D(L_39, L_40, L_41, 0, L_42, NULL);
		if (L_43)
		{
			goto IL_00e5;
		}
	}
	{
		int32_t* L_44 = ___4_entryIndex;
		int32_t L_45 = V_1;
		*((int32_t*)L_44) = (int32_t)L_45;
		return (bool)1;
	}

IL_00e5:
	{
		int32_t L_46 = V_1;
		V_0 = L_46;
		EntryU5BU5D_tA5AD9C5E098FD22F628C31033C0DAD7AD6F59635* L_47 = __this->____entries;
		int32_t L_48 = V_1;
		NullCheck(L_47);
		int32_t L_49 = ((L_47)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_48)))->___Next;
		V_1 = L_49;
	}

IL_00f9:
	{
		int32_t L_50 = V_1;
		if ((((int32_t)L_50) > ((int32_t)0)))
		{
			goto IL_0027;
		}
	}
	{
		int32_t* L_51 = ___4_entryIndex;
		int32_t L_52 = V_0;
		*((int32_t*)L_51) = (int32_t)L_52;
		return (bool)0;
	}
}
// Method Definition Index: 141132
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XHashtableState_ComputeHashCode_mD58A823B4A09F8DB93853EA9536E6B457B59F1B1_gshared (String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	{
		V_0 = ((int32_t)352654597);
		int32_t L_0 = ___1_index;
		int32_t L_1 = ___2_count;
		V_1 = ((int32_t)il2cpp_codegen_add(L_0, L_1));
		int32_t L_2 = ___1_index;
		V_2 = L_2;
		goto IL_0020;
	}

IL_000e:
	{
		int32_t L_3 = V_0;
		int32_t L_4 = V_0;
		String_t* L_5 = ___0_key;
		int32_t L_6 = V_2;
		NullCheck(L_5);
		Il2CppChar L_7;
		L_7 = String_get_Chars_mC49DF0CD2D3BE7BE97B3AD9C995BE3094F8E36D3(L_5, L_6, NULL);
		V_0 = ((int32_t)il2cpp_codegen_add(L_3, ((int32_t)(((int32_t)(L_4<<7))^(int32_t)L_7))));
		int32_t L_8 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_8, 1));
	}

IL_0020:
	{
		int32_t L_9 = V_2;
		int32_t L_10 = V_1;
		if ((((int32_t)L_9) < ((int32_t)L_10)))
		{
			goto IL_000e;
		}
	}
	{
		int32_t L_11 = V_0;
		int32_t L_12 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_subtract(L_11, ((int32_t)(L_12>>((int32_t)17)))));
		int32_t L_13 = V_0;
		int32_t L_14 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_subtract(L_13, ((int32_t)(L_14>>((int32_t)11)))));
		int32_t L_15 = V_0;
		int32_t L_16 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_subtract(L_15, ((int32_t)(L_16>>5))));
		int32_t L_17 = V_0;
		return ((int32_t)(L_17&((int32_t)2147483647LL)));
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 141127
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XHashtableState__ctor_mC2ED3CAB78829509332331B146E7165C58D3DD0F_gshared (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* __this, ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* ___0_extractKey, int32_t ___1_capacity, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		int32_t L_0 = ___1_capacity;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_1 = (Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C*)(Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C*)SZArrayNew(Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C_il2cpp_TypeInfo_var, (uint32_t)L_0);
		__this->____buckets = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____buckets), (void*)L_1);
		int32_t L_2 = ___1_capacity;
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_3 = (EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4*)(EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4*)SZArrayNew(il2cpp_rgctx_data(method->klass->rgctx_data, 1), (uint32_t)L_2);
		__this->____entries = L_3;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____entries), (void*)L_3);
		ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* L_4 = ___0_extractKey;
		__this->____extractKey = L_4;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____extractKey), (void*)L_4);
		return;
	}
}
// Method Definition Index: 141128
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* XHashtableState_Resize_m3CD152F50AD9E53B808C9B1CEC069D894A621202_gshared (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* __this, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5));
	const Il2CppFullySharedGenericAny L_13 = alloca(SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
	const Il2CppFullySharedGenericAny L_45 = L_13;
	int32_t V_0 = 0;
	XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* V_1 = NULL;
	int32_t V_2 = 0;
	int32_t V_3 = 0;
	int32_t V_4 = 0;
	int32_t V_5 = 0;
	Il2CppFullySharedGenericAny V_6 = alloca(SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
	memset(V_6, 0, SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
	{
		int32_t L_0 = __this->____numEntries;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_1 = __this->____buckets;
		NullCheck(L_1);
		if ((((int32_t)L_0) >= ((int32_t)((int32_t)(((RuntimeArray*)L_1)->max_length)))))
		{
			goto IL_0012;
		}
	}
	{
		return __this;
	}

IL_0012:
	{
		V_0 = 0;
		V_2 = 0;
		goto IL_00a7;
	}

IL_001b:
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_2 = __this->____buckets;
		int32_t L_3 = V_2;
		NullCheck(L_2);
		int32_t L_4 = L_3;
		int32_t L_5 = (L_2)->GetAt(static_cast<il2cpp_array_size_t>(L_4));
		V_3 = L_5;
		int32_t L_6 = V_3;
		if (L_6)
		{
			goto IL_009f;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_7 = __this->____buckets;
		int32_t L_8 = V_2;
		NullCheck(L_7);
		int32_t L_9;
		L_9 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717(((L_7)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_8))), (-1), 0, NULL);
		V_3 = L_9;
		goto IL_009f;
	}

IL_003d:
	{
		ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* L_10 = __this->____extractKey;
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_11 = __this->____entries;
		int32_t L_12 = V_3;
		NullCheck(L_11);
		il2cpp_codegen_memcpy(L_13, il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_11)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_12))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),0)), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		NullCheck(L_10);
		String_t* L_14;
		L_14 = InvokerFuncInvoker1< String_t*, Il2CppFullySharedGenericAny >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 6)), il2cpp_rgctx_method(method->klass->rgctx_data, 6), L_10, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5)) ? L_13: *(void**)L_13));
		if (!L_14)
		{
			goto IL_005f;
		}
	}
	{
		int32_t L_15 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_add(L_15, 1));
	}

IL_005f:
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_16 = __this->____entries;
		int32_t L_17 = V_3;
		NullCheck(L_16);
		int32_t L_18 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_16)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_17))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2));
		if (L_18)
		{
			goto IL_008d;
		}
	}
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_19 = __this->____entries;
		int32_t L_20 = V_3;
		NullCheck(L_19);
		int32_t L_21;
		L_21 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717((((int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_19)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_20))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2)))), (-1), 0, NULL);
		V_3 = L_21;
		goto IL_009f;
	}

IL_008d:
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_22 = __this->____entries;
		int32_t L_23 = V_3;
		NullCheck(L_22);
		int32_t L_24 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_22)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_23))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2));
		V_3 = L_24;
	}

IL_009f:
	{
		int32_t L_25 = V_3;
		if ((((int32_t)L_25) > ((int32_t)0)))
		{
			goto IL_003d;
		}
	}
	{
		int32_t L_26 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_26, 1));
	}

IL_00a7:
	{
		int32_t L_27 = V_2;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_28 = __this->____buckets;
		NullCheck(L_28);
		if ((((int32_t)L_27) < ((int32_t)((int32_t)(((RuntimeArray*)L_28)->max_length)))))
		{
			goto IL_001b;
		}
	}
	{
		int32_t L_29 = V_0;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_30 = __this->____buckets;
		NullCheck(L_30);
		if ((((int32_t)L_29) >= ((int32_t)((int32_t)(((int32_t)(((RuntimeArray*)L_30)->max_length))/2)))))
		{
			goto IL_00cd;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_31 = __this->____buckets;
		NullCheck(L_31);
		V_0 = ((int32_t)(((RuntimeArray*)L_31)->max_length));
		goto IL_00e2;
	}

IL_00cd:
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_32 = __this->____buckets;
		NullCheck(L_32);
		V_0 = ((int32_t)il2cpp_codegen_multiply(((int32_t)(((RuntimeArray*)L_32)->max_length)), 2));
		int32_t L_33 = V_0;
		if ((((int32_t)L_33) >= ((int32_t)0)))
		{
			goto IL_00e2;
		}
	}
	{
		OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C* L_34 = (OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C*)il2cpp_codegen_object_new(((RuntimeClass*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&OverflowException_t6F6AD8CACE20C37F701C05B373A215C4802FAB0C_il2cpp_TypeInfo_var)));
		OverflowException__ctor_m7F6A928C9BE47384586BDDE8B4B87666421E0F1A(L_34, NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_34, method);
	}

IL_00e2:
	{
		ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* L_35 = __this->____extractKey;
		int32_t L_36 = V_0;
		XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_37 = (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 0));
		((  void (*) (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*, ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD*, int32_t, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 7)))(L_37, L_35, L_36, il2cpp_rgctx_method(method->klass->rgctx_data, 7));
		V_1 = L_37;
		V_4 = 0;
		goto IL_013b;
	}

IL_00f4:
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_38 = __this->____buckets;
		int32_t L_39 = V_4;
		NullCheck(L_38);
		int32_t L_40 = L_39;
		int32_t L_41 = (L_38)->GetAt(static_cast<il2cpp_array_size_t>(L_40));
		V_5 = L_41;
		goto IL_0130;
	}

IL_0101:
	{
		XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_42 = V_1;
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_43 = __this->____entries;
		int32_t L_44 = V_5;
		NullCheck(L_43);
		il2cpp_codegen_memcpy(L_45, il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_43)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_44))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),0)), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		NullCheck(L_42);
		bool L_46;
		L_46 = InvokerFuncInvoker2< bool, Il2CppFullySharedGenericAny, Il2CppFullySharedGenericAny* >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 8)), il2cpp_rgctx_method(method->klass->rgctx_data, 8), L_42, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5)) ? L_45: *(void**)L_45), (Il2CppFullySharedGenericAny*)V_6);
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_47 = __this->____entries;
		int32_t L_48 = V_5;
		NullCheck(L_47);
		int32_t L_49 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_47)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_48))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2));
		V_5 = L_49;
	}

IL_0130:
	{
		int32_t L_50 = V_5;
		if ((((int32_t)L_50) > ((int32_t)0)))
		{
			goto IL_0101;
		}
	}
	{
		int32_t L_51 = V_4;
		V_4 = ((int32_t)il2cpp_codegen_add(L_51, 1));
	}

IL_013b:
	{
		int32_t L_52 = V_4;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_53 = __this->____buckets;
		NullCheck(L_53);
		if ((((int32_t)L_52) < ((int32_t)((int32_t)(((RuntimeArray*)L_53)->max_length)))))
		{
			goto IL_00f4;
		}
	}
	{
		XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_54 = V_1;
		return L_54;
	}
}
// Method Definition Index: 141129
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_TryGetValue_m94EE8AEAE527C34D9D2B86D03E1D04FF867266F3_gshared (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* __this, String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, Il2CppFullySharedGenericAny* ___3_value, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5));
	const Il2CppFullySharedGenericAny L_12 = alloca(SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	{
		String_t* L_0 = ___0_key;
		int32_t L_1 = ___1_index;
		int32_t L_2 = ___2_count;
		int32_t L_3;
		L_3 = ((  int32_t (*) (String_t*, int32_t, int32_t, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 10)))(L_0, L_1, L_2, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_0 = L_3;
		V_1 = 0;
		int32_t L_4 = V_0;
		String_t* L_5 = ___0_key;
		int32_t L_6 = ___1_index;
		int32_t L_7 = ___2_count;
		bool L_8;
		L_8 = ((  bool (*) (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*, int32_t, String_t*, int32_t, int32_t, int32_t*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 12)))(__this, L_4, L_5, L_6, L_7, (&V_1), il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		if (!L_8)
		{
			goto IL_0033;
		}
	}
	{
		Il2CppFullySharedGenericAny* L_9 = ___3_value;
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_10 = __this->____entries;
		int32_t L_11 = V_1;
		NullCheck(L_10);
		il2cpp_codegen_memcpy(L_12, il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_10)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_11))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),0)), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		il2cpp_codegen_memcpy((Il2CppFullySharedGenericAny*)L_9, L_12, SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 5), (void**)(Il2CppFullySharedGenericAny*)L_9, (void*)L_12);
		return (bool)1;
	}

IL_0033:
	{
		Il2CppFullySharedGenericAny* L_13 = ___3_value;
		il2cpp_codegen_initobj(L_13, SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		return (bool)0;
	}
}
// Method Definition Index: 141130
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_TryAdd_m25BEF4B433B3B23CE79C25AA27DA2FFB624CCAE2_gshared (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* __this, Il2CppFullySharedGenericAny ___0_value, Il2CppFullySharedGenericAny* ___1_newValue, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5));
	const Il2CppFullySharedGenericAny L_1 = alloca(SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
	const Il2CppFullySharedGenericAny L_17 = L_1;
	const Il2CppFullySharedGenericAny L_41 = L_1;
	const Il2CppFullySharedGenericAny L_3 = alloca(SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	String_t* V_2 = NULL;
	int32_t V_3 = 0;
	{
		Il2CppFullySharedGenericAny* L_0 = ___1_newValue;
		il2cpp_codegen_memcpy(L_1, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5)) ? ___0_value : &___0_value), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		il2cpp_codegen_memcpy((Il2CppFullySharedGenericAny*)L_0, L_1, SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 5), (void**)(Il2CppFullySharedGenericAny*)L_0, (void*)L_1);
		ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* L_2 = __this->____extractKey;
		il2cpp_codegen_memcpy(L_3, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5)) ? ___0_value : &___0_value), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		NullCheck(L_2);
		String_t* L_4;
		L_4 = InvokerFuncInvoker1< String_t*, Il2CppFullySharedGenericAny >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 6)), il2cpp_rgctx_method(method->klass->rgctx_data, 6), L_2, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5)) ? L_3: *(void**)L_3));
		V_2 = L_4;
		String_t* L_5 = V_2;
		if (L_5)
		{
			goto IL_0019;
		}
	}
	{
		return (bool)1;
	}

IL_0019:
	{
		String_t* L_6 = V_2;
		String_t* L_7 = V_2;
		NullCheck(L_7);
		int32_t L_8;
		L_8 = String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline(L_7, NULL);
		int32_t L_9;
		L_9 = ((  int32_t (*) (String_t*, int32_t, int32_t, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 10)))(L_6, 0, L_8, il2cpp_rgctx_method(method->klass->rgctx_data, 10));
		V_3 = L_9;
		int32_t* L_10 = (int32_t*)(&__this->____numEntries);
		int32_t L_11;
		L_11 = Interlocked_Increment_m3C240C32E8D9544EC050B74D4F28EEB58F1F9309(L_10, NULL);
		V_0 = L_11;
		int32_t L_12 = V_0;
		if ((((int32_t)L_12) < ((int32_t)0)))
		{
			goto IL_0042;
		}
	}
	{
		int32_t L_13 = V_0;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_14 = __this->____buckets;
		NullCheck(L_14);
		if ((((int32_t)L_13) < ((int32_t)((int32_t)(((RuntimeArray*)L_14)->max_length)))))
		{
			goto IL_0044;
		}
	}

IL_0042:
	{
		return (bool)0;
	}

IL_0044:
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_15 = __this->____entries;
		int32_t L_16 = V_0;
		NullCheck(L_15);
		il2cpp_codegen_memcpy(L_17, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5)) ? ___0_value : &___0_value), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		il2cpp_codegen_write_instance_field_data(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_15)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_16))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),0), L_17, SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_18 = __this->____entries;
		int32_t L_19 = V_0;
		NullCheck(L_18);
		int32_t L_20 = V_3;
		il2cpp_codegen_write_instance_field_data<int32_t>(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_18)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_19))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),1), L_20);
		Thread_MemoryBarrier_m83873F1E6CEB16C0781941141382DA874A36097D(NULL);
		V_1 = 0;
		goto IL_00b7;
	}

IL_0071:
	{
		int32_t L_21 = V_1;
		if (L_21)
		{
			goto IL_0095;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_22 = __this->____buckets;
		int32_t L_23 = V_3;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_24 = __this->____buckets;
		NullCheck(L_24);
		NullCheck(L_22);
		int32_t L_25 = V_0;
		int32_t L_26;
		L_26 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717(((L_22)->GetAddressAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_23&((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_24)->max_length)), 1))))))), L_25, 0, NULL);
		V_1 = L_26;
		goto IL_00ae;
	}

IL_0095:
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_27 = __this->____entries;
		int32_t L_28 = V_1;
		NullCheck(L_27);
		int32_t L_29 = V_0;
		int32_t L_30;
		L_30 = Interlocked_CompareExchange_mB06E8737D3DA41F9FFBC38A6D0583D515EFB5717((((int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_27)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_28))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2)))), L_29, 0, NULL);
		V_1 = L_30;
	}

IL_00ae:
	{
		int32_t L_31 = V_1;
		if ((((int32_t)L_31) > ((int32_t)0)))
		{
			goto IL_00b7;
		}
	}
	{
		int32_t L_32 = V_1;
		return (bool)((((int32_t)L_32) == ((int32_t)0))? 1 : 0);
	}

IL_00b7:
	{
		int32_t L_33 = V_3;
		String_t* L_34 = V_2;
		String_t* L_35 = V_2;
		NullCheck(L_35);
		int32_t L_36;
		L_36 = String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline(L_35, NULL);
		bool L_37;
		L_37 = ((  bool (*) (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*, int32_t, String_t*, int32_t, int32_t, int32_t*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 12)))(__this, L_33, L_34, 0, L_36, (&V_1), il2cpp_rgctx_method(method->klass->rgctx_data, 12));
		if (!L_37)
		{
			goto IL_0071;
		}
	}
	{
		Il2CppFullySharedGenericAny* L_38 = ___1_newValue;
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_39 = __this->____entries;
		int32_t L_40 = V_1;
		NullCheck(L_39);
		il2cpp_codegen_memcpy(L_41, il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_39)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_40))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),0)), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		il2cpp_codegen_memcpy((Il2CppFullySharedGenericAny*)L_38, L_41, SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		Il2CppCodeGenWriteBarrierForClass(il2cpp_rgctx_data(method->klass->rgctx_data, 5), (void**)(Il2CppFullySharedGenericAny*)L_38, (void*)L_41);
		return (bool)1;
	}
}
// Method Definition Index: 141131
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtableState_FindEntry_m480C6B27D99709A7E6CB50C907ACDEA057992BCD_gshared (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* __this, int32_t ___0_hashCode, String_t* ___1_key, int32_t ___2_index, int32_t ___3_count, int32_t* ___4_entryIndex, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5));
	const Il2CppFullySharedGenericAny L_16 = alloca(SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	String_t* V_2 = NULL;
	{
		int32_t* L_0 = ___4_entryIndex;
		int32_t L_1 = *((int32_t*)L_0);
		V_0 = L_1;
		int32_t L_2 = V_0;
		if (L_2)
		{
			goto IL_0020;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_3 = __this->____buckets;
		int32_t L_4 = ___0_hashCode;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_5 = __this->____buckets;
		NullCheck(L_5);
		NullCheck(L_3);
		int32_t L_6 = ((int32_t)(L_4&((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_5)->max_length)), 1))));
		int32_t L_7 = (L_3)->GetAt(static_cast<il2cpp_array_size_t>(L_6));
		V_1 = L_7;
		goto IL_00f9;
	}

IL_0020:
	{
		int32_t L_8 = V_0;
		V_1 = L_8;
		goto IL_00f9;
	}

IL_0027:
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_9 = __this->____entries;
		int32_t L_10 = V_1;
		NullCheck(L_9);
		int32_t L_11 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_9)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_10))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),1));
		int32_t L_12 = ___0_hashCode;
		if ((!(((uint32_t)L_11) == ((uint32_t)L_12))))
		{
			goto IL_00e5;
		}
	}
	{
		ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* L_13 = __this->____extractKey;
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_14 = __this->____entries;
		int32_t L_15 = V_1;
		NullCheck(L_14);
		il2cpp_codegen_memcpy(L_16, il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_14)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_15))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),0)), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		NullCheck(L_13);
		String_t* L_17;
		L_17 = InvokerFuncInvoker1< String_t*, Il2CppFullySharedGenericAny >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 6)), il2cpp_rgctx_method(method->klass->rgctx_data, 6), L_13, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 5)) ? L_16: *(void**)L_16));
		V_2 = L_17;
		String_t* L_18 = V_2;
		if (L_18)
		{
			goto IL_00c8;
		}
	}
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_19 = __this->____entries;
		int32_t L_20 = V_1;
		NullCheck(L_19);
		int32_t L_21 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_19)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_20))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2));
		if ((((int32_t)L_21) <= ((int32_t)0)))
		{
			goto IL_00e5;
		}
	}
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_22 = __this->____entries;
		int32_t L_23 = V_1;
		NullCheck(L_22);
		il2cpp_codegen_initobj((((Il2CppFullySharedGenericAny*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_22)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_23))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),0)))), SizeOf_TValue_tE0E843520D1B6FE8622D14458F0B584A2B7BCD70);
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_24 = __this->____entries;
		int32_t L_25 = V_1;
		NullCheck(L_24);
		int32_t L_26 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_24)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_25))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2));
		V_1 = L_26;
		int32_t L_27 = V_0;
		if (L_27)
		{
			goto IL_00b4;
		}
	}
	{
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_28 = __this->____buckets;
		int32_t L_29 = ___0_hashCode;
		Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* L_30 = __this->____buckets;
		NullCheck(L_30);
		int32_t L_31 = V_1;
		NullCheck(L_28);
		(L_28)->SetAt(static_cast<il2cpp_array_size_t>(((int32_t)(L_29&((int32_t)il2cpp_codegen_subtract(((int32_t)(((RuntimeArray*)L_30)->max_length)), 1))))), (int32_t)L_31);
		goto IL_00f9;
	}

IL_00b4:
	{
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_32 = __this->____entries;
		int32_t L_33 = V_0;
		NullCheck(L_32);
		int32_t L_34 = V_1;
		il2cpp_codegen_write_instance_field_data<int32_t>(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_32)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_33))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2), L_34);
		goto IL_00f9;
	}

IL_00c8:
	{
		int32_t L_35 = ___3_count;
		String_t* L_36 = V_2;
		NullCheck(L_36);
		int32_t L_37;
		L_37 = String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline(L_36, NULL);
		if ((!(((uint32_t)L_35) == ((uint32_t)L_37))))
		{
			goto IL_00e5;
		}
	}
	{
		String_t* L_38 = ___1_key;
		int32_t L_39 = ___2_index;
		String_t* L_40 = V_2;
		int32_t L_41 = ___3_count;
		int32_t L_42;
		L_42 = String_CompareOrdinal_m8940CFAE90021ED8DA3F2DF8226941C9EEB2E32D(L_38, L_39, L_40, 0, L_41, NULL);
		if (L_42)
		{
			goto IL_00e5;
		}
	}
	{
		int32_t* L_43 = ___4_entryIndex;
		int32_t L_44 = V_1;
		*((int32_t*)L_43) = (int32_t)L_44;
		return (bool)1;
	}

IL_00e5:
	{
		int32_t L_45 = V_1;
		V_0 = L_45;
		EntryU5BU5D_tD3CC9C2488DC949FC30141F89404B8BDA9DB6CE4* L_46 = __this->____entries;
		int32_t L_47 = V_1;
		NullCheck(L_46);
		int32_t L_48 = *(int32_t*)il2cpp_codegen_get_instance_field_data_pointer(((Entry_t7F8922DC9D131FDA94BF956DD509FC395285A91E*)(L_46)->GetAddressAt(static_cast<il2cpp_array_size_t>(L_47))), il2cpp_rgctx_field(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 4),2));
		V_1 = L_48;
	}

IL_00f9:
	{
		int32_t L_49 = V_1;
		if ((((int32_t)L_49) > ((int32_t)0)))
		{
			goto IL_0027;
		}
	}
	{
		int32_t* L_50 = ___4_entryIndex;
		int32_t L_51 = V_0;
		*((int32_t*)L_50) = (int32_t)L_51;
		return (bool)0;
	}
}
// Method Definition Index: 141132
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t XHashtableState_ComputeHashCode_m52BA0BD18441AD2A49C4E822AB76A7A5B7DC4B6D_gshared (String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, const RuntimeMethod* method) 
{
	int32_t V_0 = 0;
	int32_t V_1 = 0;
	int32_t V_2 = 0;
	{
		V_0 = ((int32_t)352654597);
		int32_t L_0 = ___1_index;
		int32_t L_1 = ___2_count;
		V_1 = ((int32_t)il2cpp_codegen_add(L_0, L_1));
		int32_t L_2 = ___1_index;
		V_2 = L_2;
		goto IL_0020;
	}

IL_000e:
	{
		int32_t L_3 = V_0;
		int32_t L_4 = V_0;
		String_t* L_5 = ___0_key;
		int32_t L_6 = V_2;
		NullCheck(L_5);
		Il2CppChar L_7;
		L_7 = String_get_Chars_mC49DF0CD2D3BE7BE97B3AD9C995BE3094F8E36D3(L_5, L_6, NULL);
		V_0 = ((int32_t)il2cpp_codegen_add(L_3, ((int32_t)(((int32_t)(L_4<<7))^(int32_t)L_7))));
		int32_t L_8 = V_2;
		V_2 = ((int32_t)il2cpp_codegen_add(L_8, 1));
	}

IL_0020:
	{
		int32_t L_9 = V_2;
		int32_t L_10 = V_1;
		if ((((int32_t)L_9) < ((int32_t)L_10)))
		{
			goto IL_000e;
		}
	}
	{
		int32_t L_11 = V_0;
		int32_t L_12 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_subtract(L_11, ((int32_t)(L_12>>((int32_t)17)))));
		int32_t L_13 = V_0;
		int32_t L_14 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_subtract(L_13, ((int32_t)(L_14>>((int32_t)11)))));
		int32_t L_15 = V_0;
		int32_t L_16 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_subtract(L_15, ((int32_t)(L_16>>5))));
		int32_t L_17 = V_0;
		return ((int32_t)(L_17&((int32_t)2147483647LL)));
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 141122
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XHashtable_1__ctor_m9D005D3C7669B409F1E1B4F0863596FD91DB335C_gshared (XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072* __this, ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* ___0_extractKey, int32_t ___1_capacity, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* L_0 = ___0_extractKey;
		int32_t L_1 = ___1_capacity;
		XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_2 = (XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 1));
		XHashtableState__ctor_m37E975C296D7D8CDF9ACAA9ABD7C4A0C82C427D3(L_2, L_0, L_1, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		__this->____state = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____state), (void*)L_2);
		return;
	}
}
// Method Definition Index: 141123
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtable_1_TryGetValue_mB91947F216CD72CEE9C38FA8EF56137E42817080_gshared (XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072* __this, String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, RuntimeObject** ___3_value, const RuntimeMethod* method) 
{
	{
		XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_0 = __this->____state;
		String_t* L_1 = ___0_key;
		int32_t L_2 = ___1_index;
		int32_t L_3 = ___2_count;
		RuntimeObject** L_4 = ___3_value;
		NullCheck(L_0);
		bool L_5;
		L_5 = XHashtableState_TryGetValue_m22BFB31B1A4E2CEDE4CA7133F4A0B50B9E514308(L_0, L_1, L_2, L_3, L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 5));
		return L_5;
	}
}
// Method Definition Index: 141124
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* XHashtable_1_Add_m4CE3AB9A64E8EBADEC48510932751F7C6D5581A0_gshared (XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072* __this, RuntimeObject* ___0_value, const RuntimeMethod* method) 
{
	RuntimeObject* V_0 = NULL;
	XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072* V_1 = NULL;
	bool V_2 = false;
	XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* V_3 = NULL;

IL_0000:
	{
		XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_0 = __this->____state;
		RuntimeObject* L_1 = ___0_value;
		NullCheck(L_0);
		bool L_2;
		L_2 = XHashtableState_TryAdd_m951A9A914307495745E2B33DB0BA6583FB4060A1(L_0, L_1, (&V_0), il2cpp_rgctx_method(method->klass->rgctx_data, 7));
		if (!L_2)
		{
			goto IL_0012;
		}
	}
	{
		RuntimeObject* L_3 = V_0;
		return L_3;
	}

IL_0012:
	{
		V_1 = __this;
		V_2 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_0038:
			{
				{
					bool L_4 = V_2;
					if (!L_4)
					{
						goto IL_0041;
					}
				}
				{
					XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072* L_5 = V_1;
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_5, NULL);
				}

IL_0041:
				{
					return;
				}
			}
		});
		try
		{
			XHashtable_1_t4C208624C5DAFD8858B7794DF15C5747BE4B3072* L_6 = V_1;
			Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_6, (&V_2), NULL);
			XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_7 = __this->____state;
			NullCheck(L_7);
			XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_8;
			L_8 = XHashtableState_Resize_mDB3BA5C327EF6A86F6CE58B59E1D7430E62532C5(L_7, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
			V_3 = L_8;
			Thread_MemoryBarrier_m83873F1E6CEB16C0781941141382DA874A36097D(NULL);
			XHashtableState_tA4D9621B10532B31291D60ED7E6B08342FAA537A* L_9 = V_3;
			__this->____state = L_9;
			Il2CppCodeGenWriteBarrier((void**)(&__this->____state), (void*)L_9);
			goto IL_0000;
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}
	il2cpp_codegen_no_return();
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 141122
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XHashtable_1__ctor_m76AD29BB3D4A65A3071FDFCC0AD7F927FC6051B7_gshared (XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0* __this, ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* ___0_extractKey, int32_t ___1_capacity, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD* L_0 = ___0_extractKey;
		int32_t L_1 = ___1_capacity;
		XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_2 = (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 1));
		((  void (*) (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*, ExtractKeyDelegate_t0FCB0690B76E40F52DC9D05708A8FC488624E3CD*, int32_t, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 2)))(L_2, L_0, L_1, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		__this->____state = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->____state), (void*)L_2);
		return;
	}
}
// Method Definition Index: 141123
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool XHashtable_1_TryGetValue_m2AE37A0F57ADCA202E17A64CCAF4D0F37A070A24_gshared (XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0* __this, String_t* ___0_key, int32_t ___1_index, int32_t ___2_count, Il2CppFullySharedGenericAny* ___3_value, const RuntimeMethod* method) 
{
	{
		XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_0 = __this->____state;
		String_t* L_1 = ___0_key;
		int32_t L_2 = ___1_index;
		int32_t L_3 = ___2_count;
		Il2CppFullySharedGenericAny* L_4 = ___3_value;
		NullCheck(L_0);
		bool L_5;
		L_5 = ((  bool (*) (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*, String_t*, int32_t, int32_t, Il2CppFullySharedGenericAny*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 5)))(L_0, L_1, L_2, L_3, L_4, il2cpp_rgctx_method(method->klass->rgctx_data, 5));
		return L_5;
	}
}
// Method Definition Index: 141124
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void XHashtable_1_Add_mB4B8BF6CA81EE97D92FEE9D365A8FFB15168AE00_gshared (XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0* __this, Il2CppFullySharedGenericAny ___0_value, Il2CppFullySharedGenericAny* il2cppRetVal, const RuntimeMethod* method) 
{
	const uint32_t SizeOf_TValue_t3953344BBD5AABD452C9834A9E3F75B5A767B1A7 = il2cpp_codegen_sizeof(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 6));
	const Il2CppFullySharedGenericAny L_1 = alloca(SizeOf_TValue_t3953344BBD5AABD452C9834A9E3F75B5A767B1A7);
	const Il2CppFullySharedGenericAny L_3 = L_1;
	Il2CppFullySharedGenericAny V_0 = alloca(SizeOf_TValue_t3953344BBD5AABD452C9834A9E3F75B5A767B1A7);
	memset(V_0, 0, SizeOf_TValue_t3953344BBD5AABD452C9834A9E3F75B5A767B1A7);
	XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0* V_1 = NULL;
	bool V_2 = false;
	XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* V_3 = NULL;

IL_0000:
	{
		XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_0 = __this->____state;
		il2cpp_codegen_memcpy(L_1, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 6)) ? ___0_value : &___0_value), SizeOf_TValue_t3953344BBD5AABD452C9834A9E3F75B5A767B1A7);
		NullCheck(L_0);
		bool L_2;
		L_2 = InvokerFuncInvoker2< bool, Il2CppFullySharedGenericAny, Il2CppFullySharedGenericAny* >::Invoke(il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 7)), il2cpp_rgctx_method(method->klass->rgctx_data, 7), L_0, (il2cpp_codegen_class_is_value_type(il2cpp_rgctx_data_no_init(method->klass->rgctx_data, 6)) ? L_1: *(void**)L_1), (Il2CppFullySharedGenericAny*)V_0);
		if (!L_2)
		{
			goto IL_0012;
		}
	}
	{
		il2cpp_codegen_memcpy(L_3, V_0, SizeOf_TValue_t3953344BBD5AABD452C9834A9E3F75B5A767B1A7);
		il2cpp_codegen_memcpy(il2cppRetVal, L_3, SizeOf_TValue_t3953344BBD5AABD452C9834A9E3F75B5A767B1A7);
		return;
	}

IL_0012:
	{
		V_1 = __this;
		V_2 = (bool)0;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_0038:
			{
				{
					bool L_4 = V_2;
					if (!L_4)
					{
						goto IL_0041;
					}
				}
				{
					XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0* L_5 = V_1;
					Monitor_Exit_m05B2CF037E2214B3208198C282490A2A475653FA((RuntimeObject*)L_5, NULL);
				}

IL_0041:
				{
					return;
				}
			}
		});
		try
		{
			XHashtable_1_t781B821CC6AC13BED190536310819EB7FD1463D0* L_6 = V_1;
			Monitor_Enter_m3CDB589DA1300B513D55FDCFB52B63E879794149((RuntimeObject*)L_6, (&V_2), NULL);
			XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_7 = __this->____state;
			NullCheck(L_7);
			XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_8;
			L_8 = ((  XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* (*) (XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D*, const RuntimeMethod*))il2cpp_codegen_get_direct_method_pointer(il2cpp_rgctx_method(method->klass->rgctx_data, 8)))(L_7, il2cpp_rgctx_method(method->klass->rgctx_data, 8));
			V_3 = L_8;
			Thread_MemoryBarrier_m83873F1E6CEB16C0781941141382DA874A36097D(NULL);
			XHashtableState_t34177FC58180B0A7A606129FC1FA6AF4C373043D* L_9 = V_3;
			__this->____state = L_9;
			Il2CppCodeGenWriteBarrier((void**)(&__this->____state), (void*)L_9);
			goto IL_0000;
		}
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}
	il2cpp_codegen_no_return();
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 128301
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:194>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_0 = __this->___U3CMessagesU3Ek__BackingField;
		return L_0;
	}
}
// Method Definition Index: 128302
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1_set_Messages_mD81C51A83A5A04A77DB20F2FA7BA057016AFCC66_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* ___0_value, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:194>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_0 = ___0_value;
		__this->___U3CMessagesU3Ek__BackingField = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CMessagesU3Ek__BackingField), (void*)L_0);
		return;
	}
}
// Method Definition Index: 128303
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* YieldListener_1_get_First_mBCB1DAADE9CA73AC34A776E512B4A0902A2A9D4E_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	RuntimeObject* V_0 = NULL;
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:201>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_0;
		L_0 = YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_inline(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		if (!L_0)
		{
			goto IL_0015;
		}
	}
	{
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_1;
		L_1 = YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_inline(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		NullCheck(L_1);
		int32_t L_2;
		L_2 = List_1_get_Count_m4407E4C389F22B8CEC282C15D56516658746C383_inline(L_1, il2cpp_rgctx_method(method->klass->rgctx_data, 3));
		if (L_2)
		{
			goto IL_001f;
		}
	}

IL_0015:
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:202>
		il2cpp_codegen_initobj((&V_0), sizeof(RuntimeObject*));
		RuntimeObject* L_3 = V_0;
		return L_3;
	}

IL_001f:
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:203>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_4;
		L_4 = YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_inline(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		NullCheck(L_4);
		RuntimeObject* L_5;
		L_5 = List_1_get_Item_m33561245D64798C2AB07584C0EC4F240E4839A38(L_4, 0, il2cpp_rgctx_method(method->klass->rgctx_data, 5));
		return L_5;
	}
}
// Method Definition Index: 128304
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1__ctor_m4F2224104FA5215855E744B54B9305B36ED9D31D_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:207>
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2((RuntimeObject*)__this, NULL);
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:209>
		EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3* L_0 = __this->___EventBetterInstance;
		Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87* L_1 = (Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 7));
		Action_1__ctor_m2E1DFA67718FC1A0B6E5DFEB78831FFE9C059EB4(L_1, (RuntimeObject*)__this, (intptr_t)((void*)il2cpp_rgctx_method(method->klass->rgctx_data, 6)), il2cpp_rgctx_method(method->klass->rgctx_data, 8));
		NullCheck(L_0);
		Delegate_t* L_2;
		L_2 = EventBetter_RegisterInternal_TisYieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339_TisRuntimeObject_mF3B26B943430E9D36CA501D294C18219A17C9D77(L_0, __this, L_1, (int32_t)4, il2cpp_rgctx_method(method->klass->rgctx_data, 9));
		__this->___handler = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___handler), (void*)L_2);
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:210>
		return;
	}
}
// Method Definition Index: 128305
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1_Dispose_m9C4194F200BBADB8E122CBD8749141F82A3DE8F2_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:215>
		Delegate_t* L_0 = __this->___handler;
		if (!L_0)
		{
			goto IL_002b;
		}
	}
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:217>
		EventBetter_tE8C67170281006817F883A8FA2FE3B82756E34A3* L_1 = __this->___EventBetterInstance;
		RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B L_2 = { reinterpret_cast<intptr_t> (il2cpp_rgctx_type(method->klass->rgctx_data, 10)) };
		il2cpp_codegen_runtime_class_init_inline(il2cpp_defaults.systemtype_class);
		Type_t* L_3;
		L_3 = Type_GetTypeFromHandle_m6062B81682F79A4D6DF2640692EE6D9987858C57(L_2, NULL);
		Delegate_t* L_4 = __this->___handler;
		NullCheck(L_1);
		bool L_5;
		L_5 = EventBetter_UnlistenHandler_mCFBA2C99EAD4AF0856CC60FA89B7A7C698EE5EC4(L_1, L_3, L_4, NULL);
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:218>
		__this->___handler = (Delegate_t*)NULL;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___handler), (void*)(Delegate_t*)NULL);
	}

IL_002b:
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:220>
		return;
	}
}
// Method Definition Index: 128306
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1_OnMessage_mF071E8A97B1E58D5F5007B04FD9008F26196EEC7_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, RuntimeObject* ___0_msg, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:224>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_0;
		L_0 = YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_inline(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		if (L_0)
		{
			goto IL_0013;
		}
	}
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:226>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_1 = (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D*)il2cpp_codegen_object_new(il2cpp_rgctx_data(method->klass->rgctx_data, 1));
		List_1__ctor_m7F078BB342729BDF11327FD89D7872265328F690(L_1, il2cpp_rgctx_method(method->klass->rgctx_data, 11));
		YieldListener_1_set_Messages_mD81C51A83A5A04A77DB20F2FA7BA057016AFCC66_inline(__this, L_1, il2cpp_rgctx_method(method->klass->rgctx_data, 12));
	}

IL_0013:
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:229>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_2;
		L_2 = YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_inline(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		RuntimeObject* L_3 = ___0_msg;
		NullCheck(L_2);
		List_1_Add_mEBCF994CC3814631017F46A387B1A192ED6C85C7_inline(L_2, L_3, il2cpp_rgctx_method(method->klass->rgctx_data, 13));
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:230>
		return;
	}
}
// Method Definition Index: 128307
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool YieldListener_1_System_Collections_IEnumerator_MoveNext_m21A45327488D860DB0A7A62235FB7790795A8417_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:234>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_0;
		L_0 = YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_inline(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 2));
		if (!L_0)
		{
			goto IL_0010;
		}
	}
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:236>
		YieldListener_1_Dispose_m9C4194F200BBADB8E122CBD8749141F82A3DE8F2(__this, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:237>
		return (bool)0;
	}

IL_0010:
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:240>
		return (bool)1;
	}
}
// Method Definition Index: 128308
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* YieldListener_1_System_Collections_IEnumerator_get_Current_m9B6C1A4E65630AAA6A1E6B893E7B185E716DC73C_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:245>
		return NULL;
	}
}
// Method Definition Index: 128309
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1_System_Collections_IEnumerator_Reset_mF1509740BCCD0459115341B787522F76EB68B85D_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:250>
		return;
	}
}
// Method Definition Index: 128310
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void YieldListener_1_U3C_ctorU3Eb__8_0_m4295F9B86B75A8E7A11E6187B2EF691906A07E40_gshared (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, RuntimeObject* ___0_msg, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:209>
		RuntimeObject* L_0 = ___0_msg;
		YieldListener_1_OnMessage_mF071E8A97B1E58D5F5007B04FD9008F26196EEC7(__this, L_0, il2cpp_rgctx_method(method->klass->rgctx_data, 15));
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
// Method Definition Index: 710
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline (String_t* __this, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = __this->____stringLength;
		return L_0;
	}
}
// Method Definition Index: 11935
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR Substring_t2E16755269E6716C22074D6BC0A9099915E67849 Enumerator_get_Current_m543479141C23CB5A761FFAE440388CA38F12F73A_gshared_inline (Enumerator_tE3E77E493115DC420CF9F8E1A9DBCBE1A2DF1785* __this, const RuntimeMethod* method) 
{
	{
		Substring_t2E16755269E6716C22074D6BC0A9099915E67849 L_0 = __this->____current;
		return L_0;
	}
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Func_2_Invoke_m77C320974BB0E8AFA1A7C39B514DDD8C06942AC5_gshared_inline (Func_2_t1786BA7CF27B123F6CFAA174EE698F743702757F* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) 
{
	typedef bool (*FunctionPointerType) (RuntimeObject*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA Func_2_Invoke_mD2BF1A07861A43FD1AC9DAAB47815991AD1A959F_gshared_inline (Func_2_tE6486C580FC533505BC9ECBABCA8E418D9CFD858* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) 
{
	typedef InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA (*FunctionPointerType) (RuntimeObject*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_mF4550AAF07822B89D9CEDD14EA0A726DC11B8487_gshared_inline (Func_2_t9CEA3E31C171BF402F9810DDA61E3269D41CC6BD* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) 
{
	typedef int32_t (*FunctionPointerType) (RuntimeObject*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_m168B819DF4F932C2AD05BF95F897984BE8B5B6C0_gshared_inline (Func_2_tEF1F3E6EB19300F91871059D7E435D37FEA39541* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) 
{
	typedef int32_t (*FunctionPointerType) (RuntimeObject*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 Func_2_Invoke_mA098B5996B6AE11EADA0A2F2DE377135468CEBAC_gshared_inline (Func_2_t6E897C5FF3BC8DDD4F3C18D3DFB83BDB8E4F9D2B* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) 
{
	typedef InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 (*FunctionPointerType) (RuntimeObject*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* Func_2_Invoke_m3C9BCAF1F1F54BB85749F5001D1DFDBEAB600AC8_gshared_inline (Func_2_tB87969817D1EDC637904CC3302B8C93B14ACF9FB* __this, Substring_t2E16755269E6716C22074D6BC0A9099915E67849 ___0_arg, const RuntimeMethod* method) 
{
	typedef RuntimeObject* (*FunctionPointerType) (RuntimeObject*, Substring_t2E16755269E6716C22074D6BC0A9099915E67849, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 11935
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR JsonValue_t01DB320267C848E729A400EF2345979978F851D2 Enumerator_get_Current_m9F9305DFA7957A66E104431A3EB3D5C071B4587D_gshared_inline (Enumerator_t06B71EF17663E35C7B0EA1A12263D9A5C5E116EB* __this, const RuntimeMethod* method) 
{
	{
		JsonValue_t01DB320267C848E729A400EF2345979978F851D2 L_0 = __this->____current;
		return L_0;
	}
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Func_2_Invoke_m60AC8696E0B36FEA3C3F1A9818798A3D18953D63_gshared_inline (Func_2_t93FE63D487003DC89C264F70099E05071B9C1169* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) 
{
	typedef bool (*FunctionPointerType) (RuntimeObject*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA Func_2_Invoke_m5FCD56659798D9D7163D543B4EFB36176A166CC2_gshared_inline (Func_2_t744C5C05C3CF57DDFBC4172A9D0D603C3A8512B8* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) 
{
	typedef InstanceHandle_tE6B8E29890FB27E3E1824A634157E596584E0CFA (*FunctionPointerType) (RuntimeObject*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_m321B177CDE142D1D55BBD1D63153186A54FEE249_gshared_inline (Func_2_t09E9CA48857BB30A132C819A0BF95101D519B80C* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) 
{
	typedef int32_t (*FunctionPointerType) (RuntimeObject*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t Func_2_Invoke_m42E5D81BF1869A4411FD8979982060200B5C8644_gshared_inline (Func_2_t4B9DBEFF985557367D3D1D9DEFC6C785CD39BB19* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) 
{
	typedef int32_t (*FunctionPointerType) (RuntimeObject*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 Func_2_Invoke_m7C8D770BA29067A536942979753FAB53ED84A348_gshared_inline (Func_2_t6FD5B0E57F9B999DF2B1B1566A80ECD9AF78E595* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) 
{
	typedef InternedString_t8D62A48CB7D85AAE9CFCCCFB0A77AC2844905735 (*FunctionPointerType) (RuntimeObject*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 908
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* Func_2_Invoke_mCF21D6903C1C9AB285FB0E244CDE0E9ACAC429ED_gshared_inline (Func_2_t9D6D0636D7EE282D3C55AA455C0B6CAC61A117F0* __this, JsonValue_t01DB320267C848E729A400EF2345979978F851D2 ___0_arg, const RuntimeMethod* method) 
{
	typedef RuntimeObject* (*FunctionPointerType) (RuntimeObject*, JsonValue_t01DB320267C848E729A400EF2345979978F851D2, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_arg, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 75538
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_mCDE5BAF472BC1BEBC9D091F532AC1A07D65DB0BC_gshared_inline (WorkSlice_1_tDC724EEF28BD2F2E2B6498F1FDB285F8CCF34A44* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:124>
		int32_t L_0 = __this->___m_Length;
		return L_0;
	}
}
// Method Definition Index: 75538
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_m7212817506EACBA1AB0D914DE401C6FA05F0DD9D_gshared_inline (WorkSlice_1_t95249C8534D20AFEA3985723537EDE6E5BF06809* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:124>
		int32_t L_0 = __this->___m_Length;
		return L_0;
	}
}
// Method Definition Index: 75538
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t WorkSlice_1_get_length_m0E5769CB5B052657E7327DCAD0F2CA104327D7D8_gshared_inline (WorkSlice_1_t667B566D7F9D6CCD86634FB4157540E5A9C14E87* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/com.unity.render-pipelines.universal@3b809f23691d/Runtime/LightCookieManager.cs:124>
		int32_t L_0 = __this->___m_Length;
		return L_0;
	}
}
// Method Definition Index: 141126
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* ExtractKeyDelegate_Invoke_m3214A3F92C2EB22088AB7738E8DFE1BE70925148_gshared_inline (ExtractKeyDelegate_t9BADAD4BDEEE5FEB8CBD095F1EA307C3DA921583* __this, RuntimeObject* ___0_value, const RuntimeMethod* method) 
{
	typedef String_t* (*FunctionPointerType) (RuntimeObject*, RuntimeObject*, const RuntimeMethod*);
	return ((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_value, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
// Method Definition Index: 128301
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* YieldListener_1_get_Messages_mD7A435A4750A14F899F2E69745A780ED47CA3F37_gshared_inline (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:194>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_0 = __this->___U3CMessagesU3Ek__BackingField;
		return L_0;
	}
}
// Method Definition Index: 11871
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t List_1_get_Count_m4407E4C389F22B8CEC282C15D56516658746C383_gshared_inline (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = __this->____size;
		return L_0;
	}
}
// Method Definition Index: 128302
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void YieldListener_1_set_Messages_mD81C51A83A5A04A77DB20F2FA7BA057016AFCC66_gshared_inline (YieldListener_1_tD7926F765755A644B70F8E0CEF2EEE1934CE0339* __this, List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* ___0_value, const RuntimeMethod* method) 
{
	{
		//<source_info:D:/UnityProjects/ThermalMotion/ThermalGameDemo/Assets/Photon/PhotonRealtime/Code/EventBetter.cs:194>
		List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* L_0 = ___0_value;
		__this->___U3CMessagesU3Ek__BackingField = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CMessagesU3Ek__BackingField), (void*)L_0);
		return;
	}
}
// Method Definition Index: 11882
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void List_1_Add_mEBCF994CC3814631017F46A387B1A192ED6C85C7_gshared_inline (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, RuntimeObject* ___0_item, const RuntimeMethod* method) 
{
	ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* V_0 = NULL;
	int32_t V_1 = 0;
	{
		int32_t L_0 = __this->____version;
		__this->____version = ((int32_t)il2cpp_codegen_add(L_0, 1));
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1 = __this->____items;
		V_0 = L_1;
		int32_t L_2 = __this->____size;
		V_1 = L_2;
		int32_t L_3 = V_1;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = V_0;
		NullCheck(L_4);
		if ((!(((uint32_t)L_3) < ((uint32_t)((int32_t)(((RuntimeArray*)L_4)->max_length))))))
		{
			goto IL_0034;
		}
	}
	{
		int32_t L_5 = V_1;
		__this->____size = ((int32_t)il2cpp_codegen_add(L_5, 1));
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_6 = V_0;
		int32_t L_7 = V_1;
		RuntimeObject* L_8 = ___0_item;
		NullCheck(L_6);
		(L_6)->SetAt(static_cast<il2cpp_array_size_t>(L_7), (RuntimeObject*)L_8);
		return;
	}

IL_0034:
	{
		RuntimeObject* L_9 = ___0_item;
		List_1_AddWithResize_m79A9BF770BEF9C06BE40D5401E55E375F2726CC4(__this, L_9, il2cpp_rgctx_method(method->klass->rgctx_data, 14));
		return;
	}
}

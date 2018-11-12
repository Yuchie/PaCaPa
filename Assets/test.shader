Shader "Example/Normal Extrusion" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	_Amount("Extrusion Amount", Range(-1,1)) = 0.5
	_ForceCenter("ForcePos",Vector) = (0,0,0,0)
	_CurrentCenter("CurrentPos",Vector) = (0,0,0,0)
	_ForceNormal("ForceNormal",Vector) = (0,0,0,0)
	_ForceRange("ForceRange",Float) = 1.0
	_ForceLimit("ForceLimit",Float) = 1.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
#pragma surface surf Lambert vertex:vert
		struct Input {
		float2 uv_MainTex;
	};
	float _Amount;
	float3 _ForceCenter;
	float3 _CurrentCenter;
	float3 _ForceNormal;
	float _ForceRange;
	float _ForceLimit;
	float DistanceFromPlane(float3 normalVector, float3 pointInPlane,float3 targetPoint) {
		 //平面の法線ベクトルと平面に含まれる点から平面の方程式を組み立てtargetPointの距離を計算して返す。
		//「ax+by+cz+d=0と(α,β,γ)の距離は(aα+bβ+cγ+d)/√(a^2+b^2+c^2)」である公式を利用(今回は正負も欲しいので絶対値は付けない)
		float4 abcd;
		abcd.x = normalVector.x;
		abcd.y = normalVector.y;
		abcd.z = normalVector.z;
		abcd.w = -(normalVector.x * pointInPlane.x) - (normalVector.y * pointInPlane.y) - (normalVector.z * pointInPlane.z);
		float denominator = abcd.x*abcd.x + abcd.y*abcd.y + abcd.z*abcd.z;
		float numerator = abcd.x * targetPoint.x + abcd.y * targetPoint.y + abcd.z * targetPoint.z + abcd.w;
		return numerator / sqrt(denominator);

	}
	bool isHekomi(float3 pos, float3 force) {
		//対象頂点に加わる変形が物体中心向きならtrue,そうでなければfalse
		float innerProduct = pos.x * force.x + pos.y * force.y + pos.z * force.z;
		return innerProduct > 0 ? true : false;
	}

	float distanceEffect(float3 pos, float3 forceCenter, float forceRange, float forceLimit) {
		float dis = distance(pos, forceCenter);
		if (dis < forceRange) {
			return 1;
		}
		else if (dis < forceLimit) {
			return 1 - dis / forceLimit;
		}
		else {
			return 0;
		}
	}
	void vert(inout appdata_full v) {
		
		
		v.vertex.xyz -= isHekomi(v.vertex.xyz, _ForceNormal * (DistanceFromPlane(_ForceNormal, _ForceCenter, v.vertex.xyz + _CurrentCenter)))? _ForceNormal * (DistanceFromPlane(_ForceNormal,_ForceCenter, v.vertex.xyz + _CurrentCenter)) * distanceEffect(v.vertex.xyz + _CurrentCenter, _ForceCenter,_ForceRange,_ForceLimit) : float3(0,0,0);
	}
	sampler2D _MainTex;
	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
	}
	
	ENDCG
	}
		Fallback "Diffuse"
}
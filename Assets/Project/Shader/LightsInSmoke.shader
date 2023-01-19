// 이 코드는 쉐이더랩의 코드를 유니티에 맞게 변형한 것 입니다.
// https://www.shadertoy.com/view/MdyGzR
Shader "Effect2D/LightsInSmoke"
{
	Properties
	{
		_LightColor1("LightColor1", Color) = (1.0, 0.3, 0.3, 1)
		_LightColor2("LightColor2", Color) = (0.3, 1.0, 0.3, 1)
		_LightColor3("LightColor3", Color) = (0.3, 0.3, 1.0, 1)
		
		[Space]
		_LightIntensity1("LightIntensity1", Float) = 1.0
		_LightIntensity2("LightIntensity2", Float) = 1.0
		_LightIntensity3("LightIntensity3", Float) = 1.0
		
		[Space]
		_CloudIntensity1("CloudIntensity1", Float) = 0.7
		_CloudIntensity2("CloudIntensity2", Float) = 0.7
		_CloudIntensity3("CloudIntensity3", Float) = 0.7
		
		// UI 에러 방지를 위한 프로퍼티
		[HideInInspector]
		_MainTex ("Not used texture", 2D) = "black" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct VertexInput
			{
			    fixed4 vertex : POSITION;
				fixed2 uv:TEXCOORD0;
			    fixed4 tangent : TANGENT;
			    fixed3 normal : NORMAL;
			};
			
			struct VertexOutput
			{
				fixed4 pos : SV_POSITION;
				fixed2 uv:TEXCOORD0;
			};

			//Variables
			fixed4 _LightColor1;
			fixed4 _LightColor2;
			fixed4 _LightColor3;
			
			fixed _LightIntensity1;
			fixed _LightIntensity2;
			fixed _LightIntensity3;

			fixed _CloudIntensity1;
			fixed _CloudIntensity2;
			fixed _CloudIntensity3;

			fixed3 fmod289(fixed3 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}

			fixed4 fmod289(fixed4 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}

			fixed4 permute(fixed4 x)
			{
				return fmod289(((x*34.0)+1.0)*x);
			}

			fixed4 taylorInvSqrt(fixed4 r)
			{
				return 1.79284291400159 - 0.85373472095314 * r;
			}

			fixed snoise(fixed3 v)
			{ 
				const fixed2 C = fixed2(1.0/6.0, 1.0/3.0) ;
				const fixed4 D = fixed4(0.0, 0.5, 1.0, 2.0);

				// First corner
				fixed3 i  = floor(v + dot(v, C.yyy) );
				fixed3 x0 =   v - i + dot(i, C.xxx) ;

				// Other corners
				fixed3 g = step(x0.yzx, x0.xyz);
				fixed3 l = 1.0 - g;
				fixed3 i1 = min( g.xyz, l.zxy );
				fixed3 i2 = max( g.xyz, l.zxy );

				//   x0 = x0 - 0.0 + 0.0 * C.xxx;
				//   x1 = x0 - i1  + 1.0 * C.xxx;
				//   x2 = x0 - i2  + 2.0 * C.xxx;
				//   x3 = x0 - 1.0 + 3.0 * C.xxx;
				fixed3 x1 = x0 - i1 + C.xxx;
				fixed3 x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
				fixed3 x3 = x0 - D.yyy;      // -1.0+3.0*C.x = -0.5 = -D.y

				// Permutations
				i = fmod289(i); 
				fixed4 p = permute( permute( permute( 
				         i.z + fixed4(0.0, i1.z, i2.z, 1.0 ))
				       + i.y + fixed4(0.0, i1.y, i2.y, 1.0 )) 
				       + i.x + fixed4(0.0, i1.x, i2.x, 1.0 ));

				// Gradients: 7x7 points over a square, mapped onto an octahedron.
				// The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
				fixed n_ = 0.142857142857; // 1.0/7.0
				fixed3  ns = n_ * D.wyz - D.xzx;

				fixed4 j = p - 49.0 * floor(p * ns.z * ns.z);  //  fmod(p,7*7)

				fixed4 x_ = floor(j * ns.z);
				fixed4 y_ = floor(j - 7.0 * x_ );    // fmod(j,N)

				fixed4 x = x_ *ns.x + ns.yyyy;
				fixed4 y = y_ *ns.x + ns.yyyy;
				fixed4 h = 1.0 - abs(x) - abs(y);

				fixed4 b0 = fixed4( x.xy, y.xy );
				fixed4 b1 = fixed4( x.zw, y.zw );

				//fixed4 s0 = fixed4(lessThan(b0,0.0))*2.0 - 1.0;
				//fixed4 s1 = fixed4(lessThan(b1,0.0))*2.0 - 1.0;
				fixed4 s0 = floor(b0)*2.0 + 1.0;
				fixed4 s1 = floor(b1)*2.0 + 1.0;
				fixed4 sh = -step(h, fixed4(0.0,0.0,0.0,0.0));

				fixed4 a0 = b0.xzyw + s0.xzyw*sh.xxyy ;
				fixed4 a1 = b1.xzyw + s1.xzyw*sh.zzww ;

				fixed3 p0 = fixed3(a0.xy,h.x);
				fixed3 p1 = fixed3(a0.zw,h.y);
				fixed3 p2 = fixed3(a1.xy,h.z);
				fixed3 p3 = fixed3(a1.zw,h.w);

				//Normalise gradients
				fixed4 norm = taylorInvSqrt(fixed4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
				p0 *= norm.x;
				p1 *= norm.y;
				p2 *= norm.z;
				p3 *= norm.w;

				// Mix final noise value
				fixed4 m = max(0.6 - fixed4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
				m = m * m;
				return 42.0 * dot( m*m, fixed4( dot(p0,x0), dot(p1,x1), 
				                            dot(p2,x2), dot(p3,x3) ) );
			}

			fixed normnoise(fixed noise)
			{
				return 0.5*(noise+1.0);
			}

			fixed clouds(fixed2 uv)
			{
			    uv += fixed2(_Time.y*0.05, + _Time.y*0.01);
			    
			    fixed2 off1 = fixed2(50.0,33.0);
			    fixed2 off2 = fixed2(0.0, 0.0);
			    fixed2 off3 = fixed2(-300.0, 50.0);
			    fixed2 off4 = fixed2(-100.0, 200.0);
			    fixed2 off5 = fixed2(400.0, -200.0);
			    fixed2 off6 = fixed2(100.0, -1000.0);
				fixed scale1 = 3.0;
			    fixed scale2 = 6.0;
			    fixed scale3 = 12.0;
			    fixed scale4 = 24.0;
			    fixed scale5 = 48.0;
			    fixed scale6 = 96.0;
			    return normnoise(snoise(fixed3((uv+off1)*scale1,_Time.y*0.5))*0.8 + 
			                     snoise(fixed3((uv+off2)*scale2,_Time.y*0.4))*0.4 +
			                     snoise(fixed3((uv+off3)*scale3,_Time.y*0.1))*0.2 +
			                     snoise(fixed3((uv+off4)*scale4,_Time.y*0.7))*0.1 +
			                     snoise(fixed3((uv+off5)*scale5,_Time.y*0.2))*0.05 +
			                     snoise(fixed3((uv+off6)*scale6,_Time.y*0.3))*0.025);
			}

			VertexOutput vert (VertexInput v)
			{
				VertexOutput o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = v.uv;
				//VertexFactory
				return o;
			}

			fixed3 GetCloudIntensity(fixed intensity, half clouds)
			{
				fixed val = intensity * clouds;
				return fixed3(val, val, val);
			}
	
			fixed4 frag(VertexOutput i) : SV_Target
			{
				const fixed2 uv =  i.uv/1;
				const fixed2 center = fixed2(0.5,0.5);

				const fixed2 light1 = fixed2(sin(_Time.y*1.2+50.0)*1.0 + cos(_Time.y*0.4+10.0)*0.6,sin(_Time.y*1.2+100.0)*0.8 + cos(_Time.y*0.2+20.0)*-0.2)*0.2+center;
				const fixed2 light2 = fixed2(sin(_Time.y+3.0)*-2.0,cos(_Time.y+7.0)*1.0)*0.2+center;
				const fixed2 light3 = fixed2(sin(_Time.y+3.0)*2.0,cos(_Time.y+14.0)*-1.0)*0.2+center;

				const fixed cloudIntensity1 = _CloudIntensity1 * (1.0-(2.5*distance(uv, light1)));
				const fixed lightIntensity1 = _LightIntensity1/(100.0*distance(uv,light1));

				const fixed cloudIntensity2 = _CloudIntensity2 * (1.0-(2.5*distance(uv, light2)));
				const fixed lightIntensity2 = _LightIntensity2/(100.0*distance(uv,light2));

				const fixed cloudIntensity3 = _CloudIntensity3 * (1.0-(2.5*distance(uv, light3)));
				const fixed lightIntensity3 = _LightIntensity3/(100.0*distance(uv,light3));

				return fixed4(GetCloudIntensity(cloudIntensity1, clouds(uv)) * _LightColor1.xyz + lightIntensity1 * _LightColor1.xyz +
				          GetCloudIntensity(cloudIntensity2, clouds(uv)) * _LightColor2.xyz + lightIntensity2 * _LightColor2.xyz +
				          GetCloudIntensity(cloudIntensity3, clouds(uv)) * _LightColor3.xyz + lightIntensity3 * _LightColor3.xyz 
				          ,1.0);
			}
			ENDCG
		}
	}
}


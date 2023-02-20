Shader "Custom/RenderIfNear"
{
  Properties
  {
    _Color("Color", Color) = (1,1,1,1)
    _MainTex("Albedo (RGB)", 2D) = "white" {}
    _Glossiness("Smoothness", Range(0,1)) = 0.5
    _Metallic("Metallic", Range(0,1)) = 0.0

      // Expose parameters for the minimum x, minimum z,
      // maximum x, and maximum z of the rendered volume.
      _Radius("radius", Float) = 1.0
  }
    SubShader
    {
    Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
      // Cull front
      LOD 100

      // Allow back sides of the object to render.
      // Cull Off

      CGPROGRAM

      #pragma surface surf Standard alpha:fade
      #pragma target 3.0

      sampler2D _MainTex;

      struct Input {
        float2 uv_MainTex;
        float3 worldPos;
      };

      half _Glossiness;
      half _Metallic;
      fixed4 _Color;

      // Read the min xz/ max xz material properties.
      float4 _Center;
      float _CenterDepth;
      float _Radius;

      void surf(Input IN, inout SurfaceOutputStandard o)
      {

        // Calculate a signed distance from the clipping volume.
        float outOfBounds = _Radius - length(IN.worldPos.xyz - _Center);
        clip(outOfBounds);

        // Default surface shading.
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
        o.Albedo = c.rgb;
        o.Metallic = _Metallic;
        o.Smoothness = _Glossiness;
        o.Alpha = c.a;
      }
      ENDCG
    }
      // Note that the non-clipped Diffuse material will be used for shadows.
      // If you need correct shadowing with clipped material, add a shadow pass
      // that includes the same clipping logic as above.
        FallBack "Standard"
}
using UnityEngine;
using System.Collections;
using System;

// circle packing: http://hydra.nat.uni-magdeburg.de/packing/cci/cci.html

public class Card : MonoBehaviour {

	public Symbol symbolPrefab;
	
	void Start () {
		float factor = (float) Math.Sqrt(8);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (-0.302593388348611302909204224933f*factor, -0.628341645367213738512227388956f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.302593388348611302909204224933f*factor, -0.628341645367213738512227388956f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (-0.679921171839088240043878874469f*factor, -0.155187570571975671990838057814f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.679921171839088240043878874469f*factor, -0.155187570571975671990838057814f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.000000000000000000000000000000f*factor, 0.000000000000000000000000000000f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (-0.545254445070410775447749861103f*factor, 0.434825910113495061957667559237f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.545254445070410775447749861103f*factor, 0.434825910113495061957667559237f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
		((Symbol)Instantiate (symbolPrefab, new Vector3 (0.000000000000000000000000000000f*factor,  0.697406611651388697090795775067f*factor, 0), Quaternion.identity)).transform.SetParent(this.transform);
	}

	public void setCard(int[] indexes) {
		for (int i = 0; i < 8; ++i) setSymbol(i,indexes[i]);
	}

	public void setSymbol(int symbol, int index) {
		GetComponentsInChildren<Symbol>()[symbol].SetSymbol(index);
	}
}

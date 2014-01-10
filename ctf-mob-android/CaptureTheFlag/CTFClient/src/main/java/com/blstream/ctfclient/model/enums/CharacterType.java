package com.blstream.ctfclient.model.enums;

/**
 * Created by Rafal on 08.01.14.
 */
public enum CharacterType {
	PRIVATE(0), MEDIC(1), COMMANDOS(2), SPY(3);

	private int type;

	CharacterType(int value) {
		this.type = value;
	}

	public int getType() {
		return type;
	}
}

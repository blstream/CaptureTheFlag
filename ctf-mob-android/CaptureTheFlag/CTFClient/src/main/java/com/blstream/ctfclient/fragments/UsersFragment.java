package com.blstream.ctfclient.fragments;

import android.app.Fragment;
import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.blstream.ctfclient.CTF;
import com.blstream.ctfclient.R;
import com.blstream.ctfclient.activities.GamesActivity;
import com.blstream.ctfclient.activities.LoginActivity;
import com.blstream.ctfclient.activities.MapActivity;
import com.blstream.ctfclient.activities.MapWebActivity;
import com.blstream.ctfclient.utils.SharedPreferencesUtils;

/**
 * Created by Rafał Zadrożny on 1/20/14.
 */
public class UsersFragment extends Fragment {

    private View.OnClickListener btnMapListener = new View.OnClickListener() {
        @Override
        public void onClick(View view) {
            Intent myIntent = new Intent(CTF.getStaticApplicationContext(), MapActivity.class);
            startActivity(myIntent);
        }
    };

    private View.OnClickListener btnMapWebListener = new View.OnClickListener() {
        @Override
        public void onClick(View view) {
            Intent myIntent = new Intent(CTF.getStaticApplicationContext(), MapWebActivity.class);
            startActivity(myIntent);
        }
    };

    private View.OnClickListener btnGameListener = new View.OnClickListener() {
        @Override
        public void onClick(View view) {
            Intent myIntent = new Intent(CTF.getStaticApplicationContext(), GamesActivity.class);
            startActivity(myIntent);
        }
    };

    private View.OnClickListener btnLogoutListener = new View.OnClickListener() {
        @Override
        public void onClick(View view) {
            clearToken();
            Intent myIntent = new Intent(CTF.getStaticApplicationContext(), LoginActivity.class);
            myIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TOP);
            getActivity().finish();


            startActivity(myIntent);

        }
    };

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.fragment_users, container, false);
        rootView.findViewById(R.id.map_btn).setOnClickListener(btnMapListener);
        rootView.findViewById(R.id.map_web_btn).setOnClickListener(btnMapWebListener);
        rootView.findViewById(R.id.games_btn).setOnClickListener(btnGameListener);
        rootView.findViewById(R.id.clear_token_btn).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                clearToken();
            }
        });

        rootView.findViewById(R.id.logout_btn).setOnClickListener(btnLogoutListener);
        return rootView;
    }

    private void clearToken() {
        SharedPreferencesUtils.clearToken(getActivity().getApplicationContext());
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

    }

//    private CTFRequest getUsersRequest() {
//        return new CTFRequest(
//                    Request.Method.GET,
//                    CTF.getInstance().getURL(CTFRequest.REQUEST_PARAM_USERS),
//                    createSuccessListener(),
//                    createErrorListener()
//            );
//    }
//
//    private Response.ErrorListener createErrorListener() {
//        return new Response.ErrorListener() {
//            @Override
//            public void onErrorResponse(VolleyError volleyError) {
//                Toast.makeText(CTF.getStaticApplicationContext(),
//                        ErrorHelper.getMessage(volleyError, CTF.getStaticApplicationContext()),
//                        Toast.LENGTH_LONG).show();
//                Intent myIntent = new Intent(CTF.getStaticApplicationContext(), LoginActivity.class);
//                startActivity(myIntent);
//            }
//        };
//    }
//
//    private Response.Listener<String> createSuccessListener() {
//        return new Response.Listener<String>() {
//            @Override
//            public void onResponse(String jsonObject) {
//                try {
//                    StringBuilder stringBuilder = new StringBuilder();
//                    JSONArray jsonArray = new JSONArray(jsonObject);
//                    for(int i = 0; i < jsonArray.length(); i++){
//                        User user = new Gson().fromJson(jsonArray.getString(i), User.class);
//                        stringBuilder.append("\tUser ").append(i).append("\n");
//                        stringBuilder.append("user name: ").append(user.getUserName()).append("\n");
//                        stringBuilder.append("password: ").append(user.getPassword()).append("\n");
//                        stringBuilder.append("email: ").append(user.getEmail()).append("\n");
//                        stringBuilder.append("nick: ").append(user.getNick()).append("\n");
//                        stringBuilder.append("first name: ").append(user.getFirstName()).append("\n");
//                        stringBuilder.append("last name: ").append(user.getLastName()).append("\n");
//                        if(user.getCoordinates() == null){
//                            stringBuilder.append("location: ").append("").append("\n");
//                        }else{
//                            stringBuilder.append("user name: ").append(user.getCoordinates().toString()).append("\n");
//                        }
//
//                        for(int j = 0; j < user.getCharacter().length; j++){
//                            Character character = user.getCharacter()[j];
//                            stringBuilder.append("health: ").append(character.getHealth()).append("\n");
//                            stringBuilder.append("level: ").append(character.getLevel()).append("\n");
//                            stringBuilder.append("total score: ").append(character.getTotalScore()).append("\n");
//                            stringBuilder.append("total time: ").append(character.getTotalTime()).append("\n");
//                            stringBuilder.append("type: ")
//                                    .append(CharacterType.getCharacterTypeNameForValue(character.getType()))
//                                    .append("\n");
//                        }
//                        stringBuilder.append("\n").append("##################").append("\n");
//                    }
//                    ((TextView)getView().findViewById(R.id.users)).setText(stringBuilder.toString());
//                } catch (JSONException e) {
//                    Log.e(CTF.TAG, "Error", e);
//                }
//            }
//        };
//    }

}

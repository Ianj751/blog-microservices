import { GoogleOAuthProvider, useGoogleLogin } from "@react-oauth/google";

function App() {
  const login = useGoogleLogin({
    onSuccess: (tokenResponse) => {
      console.log("Access Token:", tokenResponse.access_token);

      //TODO: Implement this in part in C#
      // Send post req w/ body containing access token, backend will retrieve data and store
      fetch("https://www.googleapis.com/oauth2/v3/userinfo", {
        headers: {
          Authorization: `Bearer ${tokenResponse.access_token}`,
        },
      })
        .then((response) => response.json())
        .then((profile) => {
          console.log("User Profile:", profile);
        });
    },
    onError: () => {
      console.error("Login Failed");
    },
  });

  return (
    <div>
      <h1>Vite + React</h1>
      <button onClick={() => login()}>Sign in with Google</button>
    </div>
  );
}
const clientId: string = import.meta.env.VITE_GOOGLE_CLIENT_ID;
//console.log(clientId);

export default function Root() {
  return (
    <GoogleOAuthProvider clientId={clientId}>
      <App />
    </GoogleOAuthProvider>
  );
}


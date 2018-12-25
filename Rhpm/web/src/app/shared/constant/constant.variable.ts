export class AppConstants {
  public static get routeLoginName(): string { return 'login'; }
  public static get AuthToken(): string { return 'auth_token'; }
  public static get ApprovalTypeRole(): string { return 'http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor'; }
  public static get RoleClaim(): string { return 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'; }
  public static get EmailClaim(): string { return 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'; }
  public static get NameClaim(): string { return 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'; }
  public static get IdClaim(): string { return 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'; }
  public static get uriForAddCandidate(): string { return '/api/Candidate/AddCandidate'; }
  public static get uriForUpdateCandidate(): string { return '/api/Candidate/UpdateCandidate'; }
  public static get uriForAddUser(): string { return '/api/User/CreateUser'; }
  public static get uriForUpdateUser(): string { return '/api/User/UpdateUser'; }
  public static get uriForUpdateUserProfile(): string { return '/api/Login/UpdateUserProfile'; }
  public static get uriForFile(): string { return '/api/Candidate/DownloadFile'; }
  public static get delayTime(): number { return 1000; }
  public static get minimumLength(): number { return 2; }
  public static get defaultValue(): number { return -1; }
  public static get keyString(): string { return 'data:image/png;base64,'; }
  public static get duplicateUserName(): string { return 'Username Already Exists'; }
  public static get duplicateEmail(): string { return 'Email Already Exists'; }
  public static get defaultProfileImage(): string { return 'assets/images/user_profile.png'; }
}

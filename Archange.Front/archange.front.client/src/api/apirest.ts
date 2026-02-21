/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

/** Account redirect model */
export interface AccountModel {
  /** Authentication url */
  authenticationUrl: string;
}

export interface BooleanStringTuple {
  item1?: boolean;
  item2?: string | null;
}

/** Country model */
export interface CountryModel {
  /**
   * Country Trigram
   * @maxLength 10
   */
  name: string;
}

/** User Gender */
export enum Gender {
  Unknown = "Unknown",
  Male = "Male",
  Female = "Female",
}

export interface Int32StringTuple {
  /** @format int32 */
  item1?: number;
  item2?: string | null;
}

/** Interest model */
export interface InterestModel {
  /**
   * Category name, references Model.Interest.InterestCategoryModel.Name
   * @maxLength 50
   */
  category: string;
  /**
   * Interest name
   * @maxLength 50
   */
  name: string;
}

/** Location Model */
export interface LocationModel {
  /**
   * Identifier
   * @format uuid
   */
  id: string;
  /**
   * Street
   * @maxLength 200
   */
  street: string;
  /**
   * Postal Code
   * @maxLength 50
   */
  postalCode: string;
  /**
   * City
   * @maxLength 50
   */
  city: string;
  /**
   * Country name, (reference Model.Contact.CountryModel.Name)
   * @maxLength 10
   */
  country: string;
}

/** Match model */
export interface MatchModel {
  /** Match Status */
  status: MatchStatus;
  /**
   * CreatedAt
   * @format date-time
   */
  createdAt?: string | null;
  /**
   * Room identifier, if status is Abstraction.Database.MatchStatus.Accepted
   * @format uuid
   */
  idRoom?: string | null;
}

/** Match Status */
export enum MatchStatus {
  Pending = "Pending",
  Accepted = "Accepted",
  Rejected = "Rejected",
}

/** Phone Number Model */
export interface PhoneNumberModel {
  /**
   * Identifier
   * @format uuid
   */
  id: string;
  /**
   * Country (references Model.Contact.CountryModel Name)
   * @maxLength 10
   */
  country: string;
  /**
   * Phone number
   * @maxLength 50
   */
  number: string;
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
}

/** Profile delete pictures model */
export interface ProfileDeletePictureModel {
  /** Pictures tro delete */
  pictures: Int32StringTuple[];
}

/** Profile model */
export interface ProfileModel {
  /**
   * Identifier
   * @format uuid
   */
  id: string;
  /**
   * Profile name
   * @maxLength 50
   */
  name: string;
  /** Profile Avatar */
  avatar: string;
  /**
   * Description
   * @maxLength 300
   */
  description: string;
  /**
   * Date of birth
   * @format date
   */
  dateOfBirth: string;
  /**
   * Lastname
   * @maxLength 25
   */
  lastName?: string | null;
  /**
   * Firstname
   * @maxLength 25
   */
  firstName?: string | null;
  /** User Gender */
  gender: Gender;
  /** Profile Pictures */
  pictures?: string[] | null;
  /** Profile interests */
  interests?: InterestModel[] | null;
  /** Profile Addresses */
  addresses?: LocationModel[] | null;
  /** Profile phones */
  phones?: PhoneNumberModel[] | null;
}

export interface StringBooleanTuple {
  item1?: string | null;
  item2?: boolean;
}

/** Forgot email model */
export interface UserForgotPasswordModel {
  /**
   * Email
   * @format email
   */
  email: string;
}

/** User model */
export interface UserModel {
  /** Email */
  email: string;
  /** Password, */
  password?: string | null;
}

/** Reset password model with token from mail */
export interface UserResetPasswordModel {
  /** Email */
  email: string;
  /** Password, */
  password?: string | null;
  /**
   * Temporary Forgot token
   * @format uuid
   */
  forgotToken: string;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key]);
    return keys
      .map((key) => (Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)))
      .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
    [ContentType.Text]: (input: any) => (input !== null && typeof input !== "string" ? JSON.stringify(input) : input),
    [ContentType.FormData]: (input: any) =>
      Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === "object" && property !== null
              ? JSON.stringify(property)
              : `${property}`,
        );
        return formData;
      }, new FormData()),
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(`${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`, {
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
      },
      signal: (cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal) || null,
      body: typeof body === "undefined" || body === null ? null : payloadFormatter(body),
    }).then(async (response) => {
      const r = response.clone() as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
        ? r
        : await response[responseFormat]()
          .then((data) => {
            if (r.ok) {
              r.data = data;
            } else {
              r.error = data;
            }
            return r;
          })
          .catch((e) => {
            r.error = e;
            return r;
          });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title No title
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  version = '1.0'
  api = {
    /**
 * @description If there is an exception on this call it means It,s probably Dependency injection
 *
 * @tags Account
 * @name AccountProvidersDetail
 * @summary Get the list of provider names
For Authentication
 * @request GET:/api/{this.version}/Account/Providers
 * @secure
 */
    accountProvidersDetail: (params: RequestParams = {}) =>
      this.request<string[], any>({
        path: `/api/${this.version}/Account/Providers`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Account
     * @name AccountLoginDetail
     * @summary OAuth login with another authority
     * @request GET:/api/{this.version}/Account/Login
     * @secure
     */
    accountLoginDetail: (
      version: string,
      query?: {
        /** provider name, should be a value from the route above */
        provider?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<AccountModel, any>({
        path: `/api/${this.version}/Account/Login`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Account
     * @name AccountLogoutDelete
     * @summary Logout
     * @request DELETE:/api/{this.version}/Account/Logout
     * @secure
     */
    accountLogoutDelete: (params: RequestParams = {}) =>
      this.request<void, ProblemDetails>({
        path: `/api/${this.version}/Account/Logout`,
        method: "DELETE",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contact
     * @name ContactCountriesDetail
     * @summary Countries
     * @request GET:/api/{this.version}/Contact/Countries
     * @secure
     */
    contactCountriesDetail: (params: RequestParams = {}) =>
      this.request<CountryModel[], any>({
        path: `/api/${this.version}/Contact/Countries`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contact
     * @name ContactPicturesDetail
     * @summary User pictures
     * @request GET:/api/{this.version}/Contact/Pictures
     * @secure
     */
    contactPicturesDetail: (params: RequestParams = {}) =>
      this.request<string[], any>({
        path: `/api/${this.version}/Contact/Pictures`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contact
     * @name ContactInterestsDetail
     * @summary Interests
     * @request GET:/api/{this.version}/Contact/Interests
     * @secure
     */
    contactInterestsDetail: (params: RequestParams = {}) =>
      this.request<InterestModel[], any>({
        path: `/api/${this.version}/Contact/Interests`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contact
     * @name ContactInterestDetail
     * @summary User interests
     * @request GET:/api/{this.version}/Contact/Interest
     * @secure
     */
    contactInterestDetail: (params: RequestParams = {}) =>
      this.request<InterestModel[], any>({
        path: `/api/${this.version}/Contact/Interest`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contact
     * @name ContactAddressesDetail
     * @summary User addresses
     * @request GET:/api/{this.version}/Contact/Addresses
     * @secure
     */
    contactAddressesDetail: (params: RequestParams = {}) =>
      this.request<LocationModel[], any>({
        path: `/api/${this.version}/Contact/Addresses`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Contact
     * @name ContactPhonesDetail
     * @summary Get user phones
     * @request GET:/api/{this.version}/Contact/Phones
     * @secure
     */
    contactPhonesDetail: (params: RequestParams = {}) =>
      this.request<PhoneNumberModel[], any>({
        path: `/api/${this.version}/Contact/Phones`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Match
     * @name MatchLikeCreate
     * @summary Like method
     * @request POST:/api/{this.version}/Match/Like
     * @secure
     */
    matchLikeCreate: (
      version: string,
      query?: {
        /** @format uuid */
        idProfile?: string;
        /** Match Status */
        status?: MatchStatus;
      },
      params: RequestParams = {},
    ) =>
      this.request<MatchModel, any>({
        path: `/api/${this.version}/Match/Like`,
        method: "POST",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Match
     * @name MatchMatchesDetail
     * @summary Get user matches
     * @request GET:/api/{this.version}/Match/Matches
     * @secure
     */
    matchMatchesDetail: (params: RequestParams = {}) =>
      this.request<MatchModel[], any>({
        path: `/api/${this.version}/Match/Matches`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Profile
     * @name ProfileGetDetail
     * @summary Get profile model
     * @request GET:/api/{this.version}/Profile/Get
     * @secure
     */
    profileGetDetail: (params: RequestParams = {}) =>
      this.request<ProfileModel, any>({
        path: `/api/${this.version}/Profile/Get`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Profile
     * @name ProfileDetailsDetail
     * @summary Get Profile by id
     * @request GET:/api/{this.version}/Profile/Details
     * @secure
     */
    profileDetailsDetail: (
      version: string,
      query?: {
        /**
         * ProfileId, which is the same as userId
         * @format uuid
         */
        profileId?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<ProfileModel, any>({
        path: `/api/${this.version}/Profile/Details`,
        method: "GET",
        query: query,
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * @description TODO Check file size, remove exif information
     *
     * @tags Profile
     * @name ProfileUploadCreate
     * @summary Upload profile pictures
     * @request POST:/api/{this.version}/Profile/Upload
     * @secure
     */
    profileUploadCreate: (
      version: string,
      data: {
        files?: File[];
      },
      query?: {
        isAvatar?: boolean;
      },
      params: RequestParams = {},
    ) =>
      this.request<BooleanStringTuple[], any>({
        path: `/api/${this.version}/Profile/Upload`,
        method: "POST",
        query: query,
        body: data,
        secure: true,
        type: ContentType.FormData,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Profile
     * @name ProfilePicturesDelete
     * @summary Delete picture
     * @request DELETE:/api/{this.version}/Profile/Pictures
     * @secure
     */
    profilePicturesDelete: (data: ProfileDeletePictureModel, params: RequestParams = {}) =>
      this.request<StringBooleanTuple[], any>({
        path: `/api/${this.version}/Profile/Pictures`,
        method: "DELETE",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserGetDetail
     * @summary Get user model
     * @request GET:/api/{this.version}/User/Get
     * @secure
     */
    userGetDetail: (params: RequestParams = {}) =>
      this.request<UserModel, any>({
        path: `/api/${this.version}/User/Get`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserForgotCreate
     * @summary Forgot password
     * @request POST:/api/{this.version}/User/Forgot
     * @secure
     */
    userForgotCreate: (data: UserForgotPasswordModel, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/${this.version}/User/Forgot`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserResetCreate
     * @summary Reset password (call forgot before to get thje token)
     * @request POST:/api/{this.version}/User/Reset
     * @secure
     */
    userResetCreate: (data: UserResetPasswordModel, params: RequestParams = {}) =>
      this.request<boolean, any>({
        path: `/api/${this.version}/User/Reset`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
}

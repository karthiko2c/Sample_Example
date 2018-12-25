import { Injectable } from '@angular/core';
import { AppConstants } from '../constant/constant.variable';
import { isNullOrUndefined } from 'util';
import { SideBarModel } from '../customModels/side-bar-model';
import { Observable } from 'rxjs/Observable';
import { HttpResponse, HttpClient } from '@angular/common/http';
import decode from 'jwt-decode';
import { Response } from '@angular/http/src/static_response';
import { ResponseContentType } from '@angular/http';
import { TranslateService } from '@ngx-translate/core';
import CryptoJS from 'crypto-js';

@Injectable()
export class UtilityService {
    admin: SideBarModel[] = [] as SideBarModel[];
    user: SideBarModel[] = [] as SideBarModel[];
    key: any;
    iv: any;

    constructor(private http: HttpClient, private translateService: TranslateService) {
        this.admin = [{ name: 'Dashboard', order: 1 }, { name: 'Users', order: 4 },
        { name: 'Skills', order: 2 }, { name: 'Qualifications', order: 3 }, { name: 'UserEventRole', order: 5 }];
        this.user = [{ name: 'Dashboard', order: 1 }, { name: 'openings', order: 2 }, { name: 'Candidates', order: 3 }];
        this.key = CryptoJS.enc.Hex.parse('0123456789abcdef0123456789abcdef');
        this.iv = CryptoJS.enc.Hex.parse('abcdef9876543210abcdef9876543210');
    }

    fillArray(n: number): any[] {
        return Array(n).fill(0).map((x, i) => i);
    }

    getLeftSideBar(): any[] {
        let tokenPayload = '';
        const token = localStorage.getItem(AppConstants.AuthToken);
        if (!isNullOrUndefined(token)) { tokenPayload = decode(token); }
        if (tokenPayload[AppConstants.RoleClaim] === 'Admin') {
            return this.admin;
        } else {
            return this.user;
        }
    }

    encrypt(plaintext: string): string {
        const text = this.padOrTruncate(plaintext);
        const encrypted = CryptoJS.AES.encrypt(text, this.key, {
            mode: CryptoJS.mode.ECB,
            padding: CryptoJS.pad.NoPadding
        });
        return encrypted.ciphertext.toString(CryptoJS.enc.Base64);
    }

    decrypt(ciphertext: string): string {
        const decrypted = CryptoJS.AES.decrypt(ciphertext, this.key, {
            mode: CryptoJS.mode.ECB,
            padding: CryptoJS.pad.NoPadding
        });
        const decryptedPassword = decrypted.toString(CryptoJS.enc.Utf8);
        return decryptedPassword.trim();
    }

    padOrTruncate(str: string): string {
        let result: string;
        result = '';
        if (str.length % 32 === 0) {
            return str;
        }
        result = str + '';
        while (!(result.length % 32 === 0)) {
            result = result + ' ';
        }
        return result;
    }

    dataURLtoFile(dataurl, filename) {
        const arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]);
        let n = bstr.length;
        const u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        return new File([u8arr], filename, { type: mime });
    }
}

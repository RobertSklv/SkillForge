import type { Metadata } from "next";
import 'bootswatch/dist/vapor/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.min.css';
import '@/styles/global.scss';
import { Header } from "@/components/header/Header";
import { ToastContainer } from "@/components/toast-container/ToastContainer";
import { GlobalDataInitializer } from "@/components/global-data-initializer/GlobalDataInitializer";
import { getCurrentUser, getReportFormOptions } from "@/lib/api/client";
import { cookies } from "next/headers";
import { JWT_TOKEN_COOKIE_NAME } from "@/lib/auth";
import { revalidateTag } from "next/cache";

export const metadata: Metadata = {
	title: "SkillForge"
};

async function revalidateAuth() {
	'use server'

	revalidateTag('auth');
}

export default async function RootLayout({ children }: Readonly<{ children: React.ReactNode; }>) {
	const cookieContext = await cookies();
	const authToken = cookieContext.get(JWT_TOKEN_COOKIE_NAME)?.value;

	const [
		currentUserInfo,
		reportFormOptions
	] = await Promise.all([
		getCurrentUser(authToken),
		getReportFormOptions(authToken),
	]);

	return (
		<html lang="en" data-bs-theme="dark">
			<body>
				<GlobalDataInitializer currentUserInfo={currentUserInfo}>
					<Header onLogout={revalidateAuth} />

					<main id="main-content" className="container pb-5">
						{children}
					</main>

					{/* <Footer />

						<CookieConsentBanner /> */}

					<ToastContainer />
				</GlobalDataInitializer>
			</body>
		</html>
	);
}

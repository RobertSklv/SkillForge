import type { Metadata } from "next";
import 'bootswatch/dist/vapor/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.min.css';
import 'skillforge-common/styles/global.scss';
import { Header } from "@/components/header/Header";
import { ToastContainer } from "@/components/toast-container/ToastContainer";
import { EnvInitializer } from "@/components/env-initializer/EnvInitializer";
import { initEnv } from "skillforge-common/env";

export const metadata: Metadata = {
	title: "SkillForge"
};

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode; }>) {
	initEnv({
		baseUrl: process.env.NEXT_PUBLIC_API_BASE_URL as string,
		backendUrl: process.env.NEXT_PUBLIC_BACKEND_DOMAIN as string,
		assetsRelativePath: ''
	})

	return (
		<html lang="en" data-bs-theme="dark">
			<body>
				<EnvInitializer>
					<Header />

					<main id="main-content" className="container pb-5">
						{children}
					</main>

					{/* <Footer />

					<CookieConsentBanner /> */}

					<ToastContainer />
				</EnvInitializer>
			</body>
		</html>
	);
}

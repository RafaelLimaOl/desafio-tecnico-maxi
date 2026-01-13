import { AppSidebar } from "@/components/app-sidebar"
import DashHeader from "@/components/dash-header"

import { SidebarProvider } from "@/components/ui/sidebar"
import { ReactNode } from "react"

export default function Layout({ children }: { children: ReactNode }) {
  return (
    <>
      <SidebarProvider>
        <AppSidebar />
        <main className="w-full">
          <DashHeader />
          <div className="px-4">{children}</div>
        </main>
      </SidebarProvider>
    </>
  )
}
